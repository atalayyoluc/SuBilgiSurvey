"use client";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ThemeProvider, createTheme, CssBaseline } from "@mui/material";
import { useState } from "react";
import { AuthCookieSync } from "@/features/auth/AuthCookieSync";

const theme = createTheme({
  palette: {
    mode: "light",
    primary: { main: "#1565c0" },
    background: {
      default: "#F6F8FC",
      paper: "#FFFFFF",
    },
    text: {
      primary: "#0F172A",
      secondary: "#475569",
    },
  },
  shape: { borderRadius: 14 },
  typography: {
    fontFamily: "var(--font-geist-sans), -apple-system, BlinkMacSystemFont, 'Segoe UI', Arial, sans-serif",
  },
  components: {
    MuiPaper: {
      styleOverrides: {
        root: {
          borderRadius: 14,
          boxShadow: "0 1px 2px rgba(15, 23, 42, 0.06)",
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 14,
          boxShadow: "0 1px 2px rgba(15, 23, 42, 0.06)",
        },
      },
    },
    MuiTableCell: {
      styleOverrides: {
        head: {
          backgroundColor: "#F1F5FB",
          color: "#0F172A",
          fontWeight: 700,
          borderBottom: "1px solid rgba(15, 23, 42, 0.08)",
        },
        body: {
          borderBottom: "1px solid rgba(15, 23, 42, 0.06)",
        },
      },
    },
    MuiTableRow: {
      styleOverrides: {
        root: {
          "&:hover": {
            backgroundColor: "#EEF4FF",
          },
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 12,
          textTransform: "none",
        },
      },
      defaultProps: {
        disableElevation: true,
      },
    },
  },
});

export function Providers({ children }: { children: React.ReactNode }) {
  const [qc] = useState(() => new QueryClient());
  return (
    <QueryClientProvider client={qc}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <AuthCookieSync />
        {children}
      </ThemeProvider>
    </QueryClientProvider>
  );
}
