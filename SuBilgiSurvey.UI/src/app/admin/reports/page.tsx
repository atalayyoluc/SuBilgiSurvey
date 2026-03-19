"use client";

import { useQuery } from "@tanstack/react-query";
import Link from "next/link";
import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  TextField,
  Typography,
  Button,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  Stack,
  TableContainer,
} from "@mui/material";
import { api } from "@/shared/api/client";
import { useState } from "react";

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

export default function ReportsListPage() {
  const [titleContains, setTitleContains] = useState("");
  const [status, setStatus] = useState<string>("");
  const [filterFrom, setFilterFrom] = useState<string>("");
  const [filterTo, setFilterTo] = useState<string>("");
  const params = new URLSearchParams();
  if (titleContains.trim()) params.set("titleContains", titleContains.trim());
  if (status !== "") params.set("status", status);
  // Tarih aralığı çakışması:
  // StartDate <= filterTo AND EndDate >= filterFrom
  if (filterFrom) params.set("endDateOnOrAfter", new Date(filterFrom).toISOString());
  if (filterTo) params.set("startDateOnOrBefore", new Date(filterTo).toISOString());

  const { data: rows = [] } = useQuery({
    queryKey: ["report-summaries", titleContains, status, filterFrom, filterTo],
    queryFn: () =>
      api
        .get(`SurveyReports${params.toString() ? `?${params}` : ""}`)
        .json<Row[]>(),
  });

  return (
    <Box>
      <Typography variant="h5" gutterBottom>
        Anket raporları
      </Typography>
      <Stack direction="row" spacing={2} sx={{ mb: 2 }}>
        <TextField
          size="small"
          label="Başlıkta ara"
          value={titleContains}
          onChange={(e) => setTitleContains(e.target.value)}
        />
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
        <FormControl size="small" sx={{ minWidth: 140 }}>
          <InputLabel>Durum</InputLabel>
          <Select
            value={status}
            label="Durum"
            onChange={(e) => setStatus(e.target.value as string)}
          >
            <MenuItem value="">Tümü</MenuItem>
            <MenuItem value="1">Aktif</MenuItem>
            <MenuItem value="0">Pasif</MenuItem>
          </Select>
        </FormControl>
      </Stack>
      <TableContainer
        component={Paper}
        variant="outlined"
        sx={{
          borderRadius: 2,
          overflow: "hidden",
          borderColor: "rgba(15, 23, 42, 0.10)",
        }}
      >
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
      </TableContainer>
    </Box>
  );
}
