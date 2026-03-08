import { keepPreviousData, useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent.ts";
import { useLocation } from "react-router";
import useAccount from "./useAccount.ts";
import useStore from "./useStore.ts";

const useActivities = (activityId?: string) => {
  const queryClient = useQueryClient();
  const { user } = useAccount();
  const location = useLocation();
  const { activityStore: {filter, startDate} } = useStore();

  const {data: activitiesGroup, isLoading, isFetchingNextPage, fetchNextPage, hasNextPage} = useInfiniteQuery<PagedList<Activity, string>>({
    queryKey: ['activities', filter, startDate],
    queryFn: async ({ pageParam = null }) => {
      const response = await agent.get<PagedList<Activity, string>>('/activities', {
        params: {
          cursor: pageParam,
          pageSize: 3,
          filter,
          startDate,
        }
      });
      return response.data;
    },
    initialPageParam: null,
    placeholderData: keepPreviousData,
    getNextPageParam: (lastPage) => lastPage.nextCursor,
    enabled: !activityId && location.pathname === '/activities' && !!user,
    // staleTime: 1000 * 60 * 5 // 5 minutes
    select: data => ({
      ...data,
      pages: data.pages.map((page) => ({
        ...page,
        items: page.items.map(activity => {
          const host = activity.attendees.find(p => p.id === activity.hostId);
          return {
            ...activity,
            isHost: user?.id === activity.hostId,
            isGoing: activity.attendees.some(attendee => attendee.id === user?.id),
            hostImageUrl: host?.imageUrl,
          }
        })
      }))
    })
  });

  const {data: activity, isLoading: isLoadingActivity} = useQuery({
    queryKey: ['activities', activityId],
    queryFn: async () => {
      const response = await agent.get<Activity>(`/activities/${activityId}`);
      return response.data;
    },
    enabled: !!activityId && !!user,
    select: data => {
      const host = data.attendees.find(p => p.id === data.hostId);
      return {
        ...data,
        isHost: data.hostId === user?.id,
        isGoing: data.attendees.some(attendee => attendee.id === user?.id),
        hostImageUrl: host?.imageUrl,
      }
    }
  })

  const updateActivity = useMutation({
    mutationFn: async (activity: Activity) => {
      await agent.put(`/activities`, activity);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities']
      })
    }
  })

  const createActivity = useMutation({
    mutationFn: async (activity: CreateActivity) => {
      const response = await agent.post(`/activities`, activity);
      return response.data;
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities']
      })
    }
  })

  const deleteActivity = useMutation(({
    mutationFn: async (activityId: string) => {
      await agent.delete(`/activities/${activityId}`);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities']
      })
    }
  }))

  const updateAttendance = useMutation({
    mutationFn: async (activityId: string) => {
      await agent.post(`/activities/${activityId}/attend`);
    },
    onMutate: async (activityId: string) => {
      const queryKey = ['activities', activityId];
      await queryClient.cancelQueries({ queryKey: queryKey });

      const prevActivity = queryClient.getQueryData<Activity>(queryKey);

      queryClient.setQueryData<Activity>(queryKey, oldActivity => {
        if (!oldActivity || !user) {
          return oldActivity;
        }

        const isHost = oldActivity.hostId === user.id;
        const isAttending = oldActivity.attendees.some(attendee => attendee.id === user?.id);

        return {
          ...oldActivity,
          isCancelled: isHost ? !oldActivity.isCancelled : oldActivity.isCancelled,
          attendees: isAttending
            ? isHost
              ? oldActivity.attendees
              : oldActivity.attendees.filter(attendee => attendee.id !== user?.id)
            : [...oldActivity.attendees, {
                id: user?.id,
                displayName: user?.displayName,
                imageUrl: user?.imageUrl
          }],
        }
      });

      return { prevActivity };
    },
    onError: (error, activityId, context) => {
      console.log(error);
      const queryKey = ['activities', activityId];
      if (context?.prevActivity){
        queryClient.setQueryData(queryKey, context.prevActivity)
      }
    }
    // onSuccess: async () => {
    //   await queryClient.invalidateQueries({
    //     queryKey: ['activities', activityId],
    //   })
    // }
  })

  return {
    activitiesGroup,
    isLoading,
    updateActivity,
    createActivity,
    deleteActivity,
    activity,
    isLoadingActivity,
    updateAttendance,
    isFetchingNextPage,
    fetchNextPage,
    hasNextPage
  };
}

export default useActivities;