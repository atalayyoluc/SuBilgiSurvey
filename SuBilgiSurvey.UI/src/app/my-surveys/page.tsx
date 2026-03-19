"use client";

import { useQuery } from "@tanstack/react-query";
import Link from "next/link";
import { useSearchParams } from "next/navigation";
import { Suspense, useMemo, useState } from "react";
import {
  Box,
  Button,
  Card,
  CardActionArea,
  CardContent,
  LinearProgress,
  Paper,
  Stack,
  Tab,
  Tabs,
  TextField,
  Typography,
} from "@mui/material";
import { api } from "@/shared/api/client";

type NewItem = {
  surveyId: number;
  title: string;
  description: string;
  startDate: string;
  endDate: string;
};

type PastItem = {
  surveyId: number;
  title: string;
  description: string;
  submittedAt: string;
};

function buildDateQuery(filterFrom: string, filterTo: string) {
  const params = new URLSearchParams();
  // StartDate <= filterTo AND EndDate >= filterFrom mapping (aynı parametre adlarıyla)
  if (filterFrom) params.set("endDateOnOrAfter", new Date(filterFrom).toISOString());
  if (filterTo) params.set("startDateOnOrBefore", new Date(filterTo).toISOString());
  return params.toString();
}

function MySurveysInner() {
  const sp = useSearchParams();
  const [tab, setTab] = useState<number>(() => (sp.get("tab") === "past" ? 1 : 0));
  const [filterFrom, setFilterFrom] = useState<string>("");
  const [filterTo, setFilterTo] = useState<string>("");

  const dateQueryString = useMemo(() => buildDateQuery(filterFrom, filterTo), [filterFrom, filterTo]);
  const qs = dateQueryString ? `?${dateQueryString}` : "";

  const isPast = tab === 1;

  const { data: newItems = [], isLoading: isLoadingNew } = useQuery({
    queryKey: ["my-surveys-new", filterFrom, filterTo],
    queryFn: () =>
      api.get(`SurveyFilling/my-surveys${qs}`).json<NewItem[]>(),
    enabled: !isPast,
  });

  const { data: pastItems = [], isLoading: isLoadingPast } = useQuery({
    queryKey: ["my-history-past", filterFrom, filterTo],
    queryFn: () =>
      api.get(`SurveyFilling/my-history${qs}`).json<PastItem[]>(),
    enabled: isPast,
  });

  const isLoading = isPast ? isLoadingPast : isLoadingNew;
  const items = isPast ? pastItems : newItems;

  const emptyText = isPast ? "Henüz gönderilmiş anketiniz yok." : "Şu an doldurabileceğiniz atanmış anket yok.";

  return (
    <Box>
      <Stack direction="row" justifyContent="space-between" alignItems="flex-start" sx={{ mb: 2 }}>
        <Box>
          <Typography variant="h5">Anketlerim</Typography>
          <Typography variant="body2" color="text.secondary">
            Yeni ve geçmiş anketler tek ekranda
          </Typography>
        </Box>
      </Stack>

      <Paper sx={{ p: 2, mb: 2 }}>
        <Tabs value={tab} onChange={(_, v) => setTab(v)} sx={{ mb: 1 }}>
          <Tab label="Yeni" />
          <Tab label="Geçmiş" />
        </Tabs>

        <Stack direction="row" spacing={2} alignItems="flex-end" flexWrap="wrap">
          <TextField
            size="small"
            label="Başlangıç"
            type="date"
            value={filterFrom}
            onChange={(e) => setFilterFrom(e.target.value)}
            InputLabelProps={{ shrink: true }}
          />
          <TextField
            size="small"
            label="Bitiş"
            type="date"
            value={filterTo}
            onChange={(e) => setFilterTo(e.target.value)}
            InputLabelProps={{ shrink: true }}
          />
          <Button
            size="small"
            variant="outlined"
            onClick={() => {
              setFilterFrom("");
              setFilterTo("");
            }}
          >
            Temizle
          </Button>
        </Stack>
      </Paper>

      {isLoading ? (
        <Box>
          <LinearProgress />
          <Typography sx={{ p: 2 }}>Yükleniyor…</Typography>
        </Box>
      ) : items.length === 0 ? (
        <Typography>{emptyText}</Typography>
      ) : (
        <Box sx={{ display: "grid", gap: 2, gridTemplateColumns: "repeat(auto-fill, minmax(280px, 1fr))" }}>
          {isPast
            ? (pastItems as PastItem[]).map((s) => (
                <Card key={s.surveyId}>
                  <CardActionArea component={Link} href={`/my-history/${s.surveyId}`}>
                    <CardContent>
                      <Typography variant="h6">{s.title}</Typography>
                      <Typography variant="body2" color="text.secondary" noWrap>
                        {s.description}
                      </Typography>
                      <Typography variant="caption" display="block" sx={{ mt: 1 }}>
                        Gönderim: {new Date(s.submittedAt).toLocaleDateString("tr")}
                      </Typography>
                    </CardContent>
                  </CardActionArea>
                </Card>
              ))
            : (newItems as NewItem[]).map((s) => (
                <Card key={s.surveyId}>
                  <CardActionArea component={Link} href={`/my-surveys/${s.surveyId}`}>
                    <CardContent>
                      <Typography variant="h6">{s.title}</Typography>
                      <Typography variant="body2" color="text.secondary" noWrap>
                        {s.description}
                      </Typography>
                      <Typography variant="caption" display="block" sx={{ mt: 1 }}>
                        {new Date(s.startDate).toLocaleDateString("tr")} —{" "}
                        {new Date(s.endDate).toLocaleDateString("tr")}
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

export default function MySurveysPage() {
  return (
    <Suspense
      fallback={
        <Box>
          <LinearProgress />
          <Typography sx={{ p: 2 }}>Yükleniyor…</Typography>
        </Box>
      }
    >
      <MySurveysInner />
    </Suspense>
  );
}
