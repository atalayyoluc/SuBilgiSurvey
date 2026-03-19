export function roleFromAccessToken(token: string): string {
  try {
    const part = token.split(".")[1];
    const json = JSON.parse(
      atob(part.replace(/-/g, "+").replace(/_/g, "/")),
    ) as Record<string, unknown>;
    const key = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    return String(json[key] ?? json.role ?? json.Role ?? "User");
  } catch {
    return "User";
  }
}
