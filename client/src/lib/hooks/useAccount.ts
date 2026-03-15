import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent.ts";
import type { LoginSchema } from "../schemas/loginSchema.ts";
import { useNavigate } from "react-router";
import type { RegisterSchema } from "../schemas/registerSchema.ts";
import { toast } from "react-toastify";
import type { ChangePasswordSchema } from "../schemas/changePasswordSchema.ts";

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

  const verifyEmail = useMutation({
    mutationFn: async ({ userId, code } : VerifyEmail) => {
      await agent.get(`/confirmEmail?userId=${userId}&code=${code}`);
    }
  })

  const resendConfirmationEmail = useMutation({
    mutationFn: async ({email, userId} : {email: string | null, userId: string | null}) => {
      await agent.get(`/account/resend-confirm-email`, {
        params: {
          email: email,
          userId: userId
        }
      })
    },
    onSuccess: () => {
      toast.success('Email sent - please check your email');
    }
  })

  const registerUser = useMutation({
    mutationFn: async (credential: RegisterSchema) => {
      await agent.post('/account/register', credential)
    },
  })

  const changePassword = useMutation({
    mutationFn: async (credential: ChangePasswordSchema) => {
      await agent.post('/account/change-password', credential);
    },
    onSuccess: () =>{
      toast.success('Your password has been changed');
    }
  })

  return {
    user,
    loadingUserInfo,
    loginUser,
    logoutUser,
    registerUser,
    verifyEmail,
    resendConfirmationEmail,
    changePassword,
  }
};


export default useAccount;