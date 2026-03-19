import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(request: NextRequest) {
  const role = request.cookies.get("sb_role")?.value
    ? decodeURIComponent(request.cookies.get("sb_role")!.value)
    : null;
  const path = request.nextUrl.pathname;

  if (path.startsWith("/admin")) {
    if (role !== "Admin") {
      return NextResponse.redirect(
        new URL(`/login?next=${encodeURIComponent(path)}`, request.url),
      );
    }
  }
  if (path.startsWith("/my-surveys") || path.startsWith("/my-history") || path.startsWith("/my-dashboard")) {
    if (!role) {
      return NextResponse.redirect(
        new URL(`/login?next=${encodeURIComponent(path)}`, request.url),
      );
    }
  }
  return NextResponse.next();
}

export const config = {
  matcher: ["/admin/:path*", "/my-surveys/:path*", "/my-history/:path*", "/my-dashboard/:path*"],
};
