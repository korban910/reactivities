import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent.ts";
import type { LoginSchema } from "../schemas/loginSchema.ts";
import { useNavigate } from "react-router";
import type { RegisterSchema } from "../schemas/registerSchema.ts";
import { toast } from "react-toastify";

const useAccount = () => {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  const { data: user, isLoading: loadingUserInfo } = useQuery({
    queryKey: ['user'],
    queryFn: async () => {
      const result = await agent.get<User>('/account/user-info');
      return result.data;
    },
    enabled: !queryClient.getQueryData(['user'])
  })

  const loginUser = useMutation({
    mutationFn: async (credential: LoginSchema) => {
      await agent.post('/login?useCookies=true', credential)
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['user'],
      });
    }
  });

  const logoutUser = useMutation({
    mutationFn: async () => {
      await agent.post('/account/logout');
    },
    onSuccess: async () => {
      queryClient.removeQueries({ queryKey: ['user'] });
      queryClient.removeQueries({ queryKey: ['activities'] });
      await navigate('/');
    }
  })

  const registerUser = useMutation({
    mutationFn: async (credential: RegisterSchema) => {
      await agent.post('/account/register', credential)
    },
    onSuccess: async () => {
      toast.success('User registered successfully - you can now login');
      navigate('/login');
    }
  })

  return {
    user,
    loadingUserInfo,
    loginUser,
    logoutUser,
    registerUser,
  }
};


export default useAccount;