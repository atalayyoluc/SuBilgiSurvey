"use client";

import { useEffect } from "react";
import { useAuthStore } from "./auth-store";

export function AuthCookieSync() {
  const role = useAuthStore((s) => s.role);
  const token = useAuthStore((s) => s.accessToken);
  useEffect(() => {
    if (typeof document === "undefined") return;
    if (token && role) {
      document.cookie = `sb_role=${encodeURIComponent(role)}; path=/; max-age=${60 * 60 * 24 * 7}; SameSite=Lax`;
    }
  }, [token, role]);
  return null;
}
