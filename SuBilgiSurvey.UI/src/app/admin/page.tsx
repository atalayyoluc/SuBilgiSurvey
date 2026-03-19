"use client";

import { useQuery } from "@tanstack/react-query";
import Link from "next/link";
import { useMemo, useState } from "react";
import {
  Box,
  Paper,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  TextField,
  Typography,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
} from "@mui/material";
import { api } from "@/shared/api/client";

type Row = {
  surveyId: number;
  title: string;
  status: number;
  startDate: string;
  endDate: string;
  assignedUserCount: number;
  submittedCount: number;
  pendingCount: number;
  completionRatePercent: number;
};

export default function AdminDashboardPage() {
  const [titleContains, setTitleContains] = useState("");
  const [status, setStatus] = useState<string>("");
  const [filterFrom, setFilterFrom] = useState<string>("");
  const [filterTo, setFilterTo] = useState<string>("");

  const queryString = useMemo(() => {
    const params = new URLSearchParams();
    if (titleContains.trim()) params.set("titleContains", titleContains.trim());
    if (status !== "") params.set("status", status);

    // Tarih aralığı çakışması:
    // StartDate <= filterTo AND EndDate >= filterFrom
    if (filterFrom) params.set("endDateOnOrAfter", new Date(filterFrom).toISOString());
    if (filterTo) params.set("startDateOnOrBefore", new Date(filterTo).toISOString());

    return params.toString();
  }, [titleContains, status, filterFrom, filterTo]);

  const { data: rows = [] } = useQuery({
    queryKey: ["admin-dashboard", titleContains, status, filterFrom, filterTo],
    queryFn: () =>
      api
        .get(`SurveyReports${queryString ? `?${queryString}` : ""}`)
        .json<Row[]>(),
  });

  const completionSummary = useMemo(() => {
    const total = rows.length;
    const avg = total === 0 ? 0 : rows.reduce((a, r) => a + r.completionRatePercent, 0) / total;
    return { total, avg: Math.round(avg * 10) / 10 };
  }, [rows]);

  return (
    <Box>
      <Stack direction="row" spacing={2} alignItems="flex-start" justifyContent="space-between" sx={{ mb: 2 }}>
        <Box>
          <Typography variant="h5">Dashboard</Typography>
          <Typography variant="body2" color="text.secondary">
            Anket rapor özetleri (filtreli)
          </Typography>
          <Typography variant="caption" display="block" sx={{ mt: 1 }}>
            {completionSummary.total} anket kaydı, ort. tamamlama %{completionSummary.avg}
          </Typography>
        </Box>
        <Box />
      </Stack>

      <Paper sx={{ p: 2, mb: 2 }}>
        <Stack direction="row" spacing={2} alignItems="flex-end" flexWrap="wrap">
          <TextField
            size="small"
            label="Başlıkta ara"
            value={titleContains}
            onChange={(e) => setTitleContains(e.target.value)}
          />

          <FormControl size="small" sx={{ minWidth: 160 }}>
            <InputLabel>Durum</InputLabel>
            <Select label="Durum" value={status} onChange={(e) => setStatus(e.target.value as string)}>
              <MenuItem value="">Tümü</MenuItem>
              <MenuItem value="1">Aktif</MenuItem>
              <MenuItem value="0">Pasif</MenuItem>
            </Select>
          </FormControl>

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
              setTitleContains("");
              setStatus("");
              setFilterFrom("");
              setFilterTo("");
            }}
          >
            Temizle
          </Button>
        </Stack>
      </Paper>

      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Başlık</TableCell>
            <TableCell>Atanan</TableCell>
            <TableCell>Gönderen</TableCell>
            <TableCell>Bekleyen</TableCell>
            <TableCell>%</TableCell>
            <TableCell align="right">Detay</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {rows.map((r) => (
            <TableRow key={r.surveyId}>
              <TableCell>{r.title}</TableCell>
              <TableCell>{r.assignedUserCount}</TableCell>
              <TableCell>{r.submittedCount}</TableCell>
              <TableCell>{r.pendingCount}</TableCell>
              <TableCell>{r.completionRatePercent}%</TableCell>
              <TableCell align="right">
                <Button component={Link} href={`/admin/reports/${r.surveyId}`} size="small">
                  Aç
                </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </Box>
  );
}

