"use client";

import { useState, Suspense } from "react";
import { useRouter, useSearchParams } from "next/navigation";
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
  email: z.string().email(),
  password: z.string().min(1),
});

type Form = z.infer<typeof schema>;

type AuthResponse = {
  accessToken: { token: string; expires: string };
  refreshToken: string;
};

function LoginForm() {
  const router = useRouter();
  const params = useSearchParams();
  const next = params.get("next") ?? "";
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
        .post(`${getApiBase()}/Auth/Login`, { json: data })
        .json<AuthResponse>();
      setFrom(res.accessToken.token, res.refreshToken);
      const r = useAuthStore.getState().role;
      if (next) router.push(next);
      else router.push(r === "Admin" ? "/admin" : "/my-dashboard");
    } catch (e) {
      setErr(await readProblem(e));
    }
  };

  return (
    <Paper sx={{ p: 3, maxWidth: 400, mx: "auto", mt: 4 }}>
      <Typography variant="h5" gutterBottom>
        Giriş
      </Typography>
      {err && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {err}
        </Alert>
      )}
      <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate>
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
          label="Şifre"
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
          Giriş
        </Button>
        <Button component={Link} href="/register" fullWidth sx={{ mt: 1 }}>
          Kayıt ol
        </Button>
      </Box>
    </Paper>
  );
}

export default function LoginPage() {
  return (
    <Suspense fallback={<Box sx={{ p: 4 }}>Yükleniyor…</Box>}>
      <LoginForm />
    </Suspense>
  );
}
