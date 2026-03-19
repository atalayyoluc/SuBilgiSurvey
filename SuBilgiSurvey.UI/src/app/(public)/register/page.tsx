"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import {
  Box,
  Button,
  TextField,
  Typography,
  Paper,
  Alert,
} from "@mui/material";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import ky from "ky";
import { getApiBase } from "@/shared/config/env";
import { useAuthStore } from "@/features/auth/auth-store";
import { readProblem } from "@/shared/api/client";

const schema = z.object({
  fullName: z.string().min(1).max(200),
  email: z.string().email(),
  password: z.string().min(6).max(100),
});

type Form = z.infer<typeof schema>;

type AuthResponse = {
  accessToken: { token: string; expires: string };
  refreshToken: string;
};

export default function RegisterPage() {
  const router = useRouter();
  const setFrom = useAuthStore((s) => s.setFromAuthResult);
  const [err, setErr] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<Form>({ resolver: zodResolver(schema) });

  const onSubmit = async (data: Form) => {
    setErr(null);
    try {
      const res = await ky
        .post(`${getApiBase()}/Auth/Register`, { json: data })
        .json<AuthResponse>();
      setFrom(res.accessToken.token, res.refreshToken);
      router.push("/my-surveys");
    } catch (e) {
      setErr(await readProblem(e));
    }
  };

  return (
    <Paper sx={{ p: 3, maxWidth: 400, mx: "auto", mt: 4 }}>
      <Typography variant="h5" gutterBottom>
        Kayıt
      </Typography>
      {err && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {err}
        </Alert>
      )}
      <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate>
        <TextField
          fullWidth
          label="Ad Soyad"
          margin="normal"
          {...register("fullName")}
          error={!!errors.fullName}
          helperText={errors.fullName?.message}
        />
        <TextField
          fullWidth
          label="E-posta"
          margin="normal"
          {...register("email")}
          error={!!errors.email}
          helperText={errors.email?.message}
        />
        <TextField
          fullWidth
          label="Şifre (min 6)"
          type="password"
          margin="normal"
          {...register("password")}
          error={!!errors.password}
          helperText={errors.password?.message}
        />
        <Button
          type="submit"
          fullWidth
          variant="contained"
          sx={{ mt: 2 }}
          disabled={isSubmitting}
        >
          Kayıt ol
        </Button>
        <Button component={Link} href="/login" fullWidth sx={{ mt: 1 }}>
          Giriş
        </Button>
      </Box>
    </Paper>
  );
}
