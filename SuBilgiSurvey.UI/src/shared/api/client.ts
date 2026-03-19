import ky, { HTTPError } from "ky";
import { getApiBase } from "@/shared/config/env";
import { getAccessToken } from "@/features/auth/auth-store";

export const api = ky.create({
  prefixUrl: getApiBase(),
  hooks: {
    beforeRequest: [
      (request) => {
        const t = getAccessToken();
        if (t) request.headers.set("Authorization", `Bearer ${t}`);
      },
    ],
  },
});

export async function apiRefresh(refreshToken: string) {
  return ky
    .post(`${getApiBase()}/Auth/Refresh`, {
      headers: { Authorization: `Bearer ${refreshToken}` },
    })
    .json<{ token: string; expires: string }>();
}

export type ProblemDetails = {
  title?: string;
  detail?: string;
  status?: number;
};

export async function readProblem(e: unknown): Promise<string> {
  if (e instanceof HTTPError) {
    try {
      const j = (await e.response.json()) as ProblemDetails;
      return j.detail ?? j.title ?? e.message;
    } catch {
      return e.message;
    }
  }
  return e instanceof Error ? e.message : "Bilinmeyen hata";
}
