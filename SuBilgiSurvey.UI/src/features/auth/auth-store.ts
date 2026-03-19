"use client";

import { create } from "zustand";
import { persist } from "zustand/middleware";
import { roleFromAccessToken } from "./jwt";

type AuthState = {
  accessToken: string | null;
  refreshToken: string | null;
  role: string | null;
  setFromAuthResult: (accessToken: string, refreshToken: string) => void;
  logout: () => void;
};

function setRoleCookie(role: string) {
  if (typeof document === "undefined") return;
  document.cookie = `sb_role=${encodeURIComponent(role)}; path=/; max-age=${60 * 60 * 24 * 7}; SameSite=Lax`;
}

function clearRoleCookie() {
  if (typeof document === "undefined") return;
  document.cookie = "sb_role=; path=/; max-age=0";
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      accessToken: null,
      refreshToken: null,
      role: null,
      setFromAuthResult: (accessToken, refreshToken) => {
        const role = roleFromAccessToken(accessToken);
        setRoleCookie(role);
        set({ accessToken, refreshToken, role });
      },
      logout: () => {
        clearRoleCookie();
        set({ accessToken: null, refreshToken: null, role: null });
      },
    }),
    {
      name: "sb-auth",
      partialize: (s) => ({
        accessToken: s.accessToken,
        refreshToken: s.refreshToken,
        role: s.role,
      }),
    },
  ),
);

export function getAccessToken(): string | null {
  return useAuthStore.getState().accessToken;
}
