"use client";

import { Box, Button, Stack, Typography } from "@mui/material";
import Link from "next/link";

export default function HomePage() {
  return (
    <Box sx={{ p: 4, maxWidth: 560, mx: "auto" }}>
      <Typography variant="h4" gutterBottom>
        Su Bilgi Survey
      </Typography>
      <Typography color="text.secondary" sx={{ mb: 3 }}>
        Giriş yapın veya kayıt olun.
      </Typography>
      <Stack direction="row" spacing={2}>
        <Button component={Link} href="/login" variant="contained">
          Giriş
        </Button>
        <Button component={Link} href="/register" variant="outlined">
          Kayıt
        </Button>
      </Stack>
    </Box>
  );
}
