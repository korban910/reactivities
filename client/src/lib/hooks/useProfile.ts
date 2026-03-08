import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent.ts";
import { useMemo } from "react";
import type { EditProfileSchema } from "../schemas/editProfileSchema.ts";

const useProfile = (
  userId?: string,
  predicate?: string,
  filter?: string) => {
  const queryClient = useQueryClient();

  const {data: profile, isLoading: loadingProfile} = useQuery<Profile>({
    queryKey: ['profile', userId],
    queryFn: async () => {
      const response = await agent.get<Profile>(`/profiles/${userId}`);
      return response.data;
    },
    enabled: !!userId && !predicate && !filter,
  });

  const {data: photos, isLoading: loadingPhotos} = useQuery<Photo[]>({
    queryKey: ['photos', userId],
    queryFn: async() => {
      const response = await agent.get<Photo[]>(`/profiles/${userId}/photos`);
      return response.data;
    },
    enabled: !!userId && !predicate && !filter
  });

  const { data: userActivities, isLoading: loadingUserActivities } = useQuery<UserActivity[]>({
    queryKey: ['user-activities', userId, filter],
    queryFn: async () => {
      const response = await agent.get<UserActivity[]>(`/profiles/${userId}/activities`, {
        params: {
          filter
        }
      })
      return response.data;
    },
    enabled: !!userId && !!filter,
  })

  const { data: followings, isLoading: loadingFollowings } = useQuery<Profile[]>({
    queryKey: ['followings', userId, predicate],
    queryFn: async () => {
      const response = await agent.get<Profile[]>(`/profiles/${userId}/follow-list?predicate=${predicate}`);
      return response.data;
    },
    enabled: !!userId && !!predicate && !filter
  })

  const uploadPhoto = useMutation({
    mutationFn: async (file: Blob) => {
      const formData = new FormData();
      formData.append("file", file);
      const response = await agent.post<Photo>(`/profiles/add-photo`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        }
      });
      return response.data;
    },
    onSuccess: async (photo: Photo) => {
      await queryClient.invalidateQueries({
        queryKey: ['photos', userId],
      });

      queryClient.setQueryData(['user'], (data: User) => {
        if (!data) return data
        return {
          ...data,
          imageUrl: data.imageUrl ?? photo.url
        };
      });

      queryClient.setQueryData(['profile', userId], (data: Profile) => {
        if (!data) return data
        return {
          ...data,
          imageUrl: data.imageUrl ?? photo.url
        };
      });
    }
  })

  const setMainPhoto = useMutation({
    mutationFn: async (photo: Photo) => {
      await agent.put(`/profiles/${photo.id}/set-main-photo`);
    },
    onSuccess: async (_, photo: Photo) => {
      queryClient.setQueryData(['user'], (data: User) => {
        if (!data) return data
        return {
          ...data,
          imageUrl: photo.url
        }
      })

      queryClient.setQueryData(['profile', userId], (data: Profile) => {
        if (!data) return data
        return {
          ...data,
          imageUrl: photo.url
        }
      })
    }
  })

  const editProfile = useMutation({
    mutationFn: async (data: EditProfileSchema) => {
      const response = await agent.put(`/profiles/edit-profile`, data);
      return response.data;
    },
    onSuccess: async (_, data: EditProfileSchema) => {
      queryClient.setQueryData(['user'], (user: User) => {
        if (!user) return user
        return {
          ...user,
          displayName: data.displayName,
          bio: data.bio,
        }
      })

      queryClient.setQueryData(['profile', userId], (profile: Profile) => {
        if (!profile) return profile
        return {
          ...profile,
          displayName: data.displayName,
          bio: data.bio,
        }
      })
    }
  })

  const deletePhoto = useMutation({
    mutationFn: async (photoId: string) => {
      await agent.delete(`/profiles/${photoId}/photos`);
    },
    onSuccess: (_, photoId: string) => {
      queryClient.setQueryData(['photos', userId], (data: Photo[]) => {
        if (!data) return data;
        return data.filter((photo: Photo) => photo.id !== photoId);
      })
    }
  })

  const updateFollowing = useMutation({
    mutationFn: async() => {
      await agent.post(`/profiles/${userId}/follow`)
    },
    onSuccess: () => {
      queryClient.setQueryData(['profile', userId], (profile: Profile) => {
        queryClient.invalidateQueries({
          queryKey: ['followings', userId, 'followers'],
        })
        if (!profile || profile.followersCount === undefined) return profile
        return {
          ...profile,
          following: !profile.following,
          followersCount: profile.following ? (profile?.followersCount - 1) : (profile?.followersCount + 1),
        }
      })
    }
  })

  const isCurrentUser = useMemo(() => {
    return userId === queryClient.getQueryData<User>(['user'])?.id
  },[userId, queryClient])

  return {
    profile,
    loadingProfile,
    photos,
    loadingPhotos,
    isCurrentUser,
    uploadPhoto,
    setMainPhoto,
    deletePhoto,
    editProfile,
    updateFollowing,
    followings,
    loadingFollowings,
    userActivities,
    loadingUserActivities
  }
}

export {
  useProfile,
}