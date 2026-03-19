"use client";

import SidebarLayout from "@/components/layout/SidebarLayout";

const navLinks = [
  { href: "/admin", label: "Dashboard" },
  { href: "/admin/answer-templates", label: "Cevap şablonları" },
  { href: "/admin/questions", label: "Sorular" },
  { href: "/admin/surveys", label: "Anketler" },
  { href: "/admin/reports", label: "Raporlar" },
];

export default function AdminLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return <SidebarLayout navLinks={navLinks} brandLabel="Admin Paneli">{children}</SidebarLayout>;
}
