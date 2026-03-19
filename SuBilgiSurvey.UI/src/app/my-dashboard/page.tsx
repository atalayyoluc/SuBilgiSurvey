"use client";

import { useQuery } from "@tanstack/react-query";
import Link from "next/link";
import { api } from "@/shared/api/client";
import {
  Box,
  Button,
  Card,
  CardActionArea,
  CardContent,
  LinearProgress,
  Paper,
  Stack,
  Typography,
} from "@mui/material";

type AssignedItem = {
  surveyId: number;
  title: string;
};

type SubmittedItem = {
  surveyId: number;
  title: string;
  description: string;
  submittedAt: string;
};

function DashboardMetricCard({
  title,
  value,
  subtitle,
}: {
  title: string;
  value: string | number;
  subtitle?: string;
}) {
  return (
    <Paper sx={{ p: 2, flex: 1 }}>
      <Typography variant="body2" color="text.secondary">
        {title}
      </Typography>
      <Typography variant="h4" sx={{ mt: 0.5 }}>
        {value}
      </Typography>
      {subtitle && (
        <Typography variant="caption" display="block" sx={{ mt: 0.5 }}>
          {subtitle}
        </Typography>
      )}
    </Paper>
  );
}

export default function MyDashboardPage() {
  const { data: assigned = [], isLoading: assignedLoading } = useQuery({
    queryKey: ["dashboard-assigned"],
    queryFn: () => api.get("SurveyFilling/my-surveys").json<AssignedItem[]>(),
  });

  const { data: submitted = [], isLoading: submittedLoading } = useQuery({
    queryKey: ["dashboard-submitted"],
    queryFn: () => api.get("SurveyFilling/my-history").json<SubmittedItem[]>(),
  });

  const isLoading = assignedLoading || submittedLoading;
  const solvedCount = submitted.length;
  const pendingCount = assigned.length;
  const latest = submitted.slice(0, 5);

  if (isLoading) {
    return (
      <Box>
        <LinearProgress />
        <Typography sx={{ p: 2 }}>Yükleniyor…</Typography>
      </Box>
    );
  }

  return (
    <Box>
      <Stack direction="row" spacing={2} sx={{ mb: 2, flexWrap: "wrap" }}>
        <DashboardMetricCard title="Yeni anketler" value={pendingCount} subtitle="Doldurmanız bekleyenler" />
        <DashboardMetricCard title="Geçmiş anketler" value={solvedCount} subtitle="Gönderdiğiniz anketler" />
        <DashboardMetricCard title="Toplam çözdüğüm" value={solvedCount} />
      </Stack>

      <Stack direction="row" justifyContent="space-between" alignItems="center" sx={{ mb: 1 }}>
        <Typography variant="h6">Son gönderimler</Typography>
        <Button size="small" variant="outlined" component={Link} href="/my-surveys?tab=past">
          Tümü
        </Button>
      </Stack>

      {latest.length === 0 ? (
        <Typography color="text.secondary">Henüz gönderilmiş anketiniz yok.</Typography>
      ) : (
        <Box sx={{ display: "grid", gap: 2, gridTemplateColumns: "repeat(auto-fill, minmax(280px, 1fr))" }}>
          {latest.map((s) => (
            <Card key={s.surveyId}>
              <CardActionArea component={Link} href={`/my-history/${s.surveyId}`}>
                <CardContent>
                  <Typography variant="h6">{s.title}</Typography>
                  <Typography variant="body2" color="text.secondary" noWrap>
                    {s.description}
                  </Typography>
                  <Typography variant="caption" display="block" sx={{ mt: 1 }}>
                    Gönderim: {new Date(s.submittedAt).toLocaleString("tr")}
                  </Typography>
                </CardContent>
              </CardActionArea>
            </Card>
          ))}
        </Box>
      )}
    </Box>
  );
}

