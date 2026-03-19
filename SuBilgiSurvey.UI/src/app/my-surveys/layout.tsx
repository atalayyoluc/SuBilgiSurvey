"use client";

import SidebarLayout from "@/components/layout/SidebarLayout";

export default function UserLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const navLinks = [
    { href: "/my-dashboard", label: "Dashboard" },
    { href: "/my-surveys", label: "Anketlerim" },
  ];

  return <SidebarLayout navLinks={navLinks} brandLabel="Kullanıcı Paneli">{children}</SidebarLayout>;
}
