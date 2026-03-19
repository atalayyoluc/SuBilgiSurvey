"use client";

import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import { useAuthStore } from "@/features/auth/auth-store";
import {
  Box,
  Button,
  Divider,
  Drawer,
  List,
  ListItemButton,
  ListItemText,
  Toolbar,
  Typography,
} from "@mui/material";

type NavLink = { href: string; label: string };

export default function SidebarLayout({
  children,
  navLinks,
  brandLabel = "Su Bilgi Survey",
}: {
  children: React.ReactNode;
  navLinks: NavLink[];
  brandLabel?: string;
}) {
  const pathname = usePathname();
  const router = useRouter();
  const logout = useAuthStore((s) => s.logout);

  const drawerWidth = 260;

  const isActive = (href: string) => {
    // "/admin" gibi section root linkleri alt sayfalarda da seçili kalmasın.
    // Sadece tam eşleşmede aktif olsun.
    if (href === "/admin" || href === "/my-dashboard") {
      return pathname === href;
    }
    if (pathname === href) return true;
    // href örn: "/admin" ise "/admin/reports/1" gibi alt sayfaları da aktif say.
    return pathname.startsWith(href.endsWith("/") ? href : `${href}/`);
  };

  return (
    <Box sx={{ display: "flex", minHeight: "100dvh", bgcolor: "background.default" }}>
      <Drawer
        variant="permanent"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          [`& .MuiDrawer-paper`]: {
            width: drawerWidth,
            boxSizing: "border-box",
            border: "none",
            borderRight: "1px solid rgba(15, 23, 42, 0.08)",
            bgcolor: "background.paper",
          },
        }}
      >
        <Toolbar sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", px: 2 }}>
          <Typography variant="subtitle1" fontWeight={800} noWrap>
            {brandLabel}
          </Typography>
        </Toolbar>
        <Divider />

        <List sx={{ p: 1 }}>
          {navLinks.map((l) => {
            const active = isActive(l.href);
            return (
              <ListItemButton
                key={l.href}
                component={Link}
                href={l.href}
                selected={active}
                sx={{
                  my: 0.5,
                  mx: 0.25,
                  borderRadius: 2,
                  "&.Mui-selected": {
                    bgcolor: "#EEF4FF",
                    color: "#1565c0",
                    "&:hover": { bgcolor: "#E5EEFF" },
                  },
                }}
              >
                <ListItemText primary={l.label} />
              </ListItemButton>
            );
          })}
        </List>

        <Box sx={{ mt: "auto", p: 1.5 }}>
          <Button
            fullWidth
            variant="contained"
            color="error"
            onClick={() => {
              logout();
              router.push("/login");
            }}
          >
            Çıkış
          </Button>
        </Box>
      </Drawer>

      <Box component="main" sx={{ flexGrow: 1, p: 3 }}>
        {children}
      </Box>
    </Box>
  );
}

