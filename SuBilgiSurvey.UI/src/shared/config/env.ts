export function getApiBase(): string {
  const u = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5083";
  return u.replace(/\/$/, "");
}
