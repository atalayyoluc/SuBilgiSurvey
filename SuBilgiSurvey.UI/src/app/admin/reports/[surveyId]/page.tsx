"use client";

import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import Link from "next/link";
import {
  Box,
  Button,
  Typography,
  TextField,
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Stack,
  Chip,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Paper,
  Divider,
  LinearProgress,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { BarChart } from "@mui/x-charts/BarChart";
import { api } from "@/shared/api/client";
import { useState } from "react";

type Summary = {
  totalAssignedUsers: number;
  totalSubmitted: number;
  totalPending: number;
  completionRatePercent: number;
};
type QStat = {
  questionId: number;
  questionText: string;
  sortOrder: number;
  optionCounts: {
    answerTemplateOptionId: number;
    optionText: string;
    responseCount: number;
  }[];
};
type UserS = { userId: number; fullName: string; submittedAt: string | null };
type Ans = {
  questionId: number;
  questionText: string;
  questionSortOrder: number;
  selectedOptionText: string;
};
type UserAns = {
  userId: number;
  fullName: string;
  submittedAt: string;
  answers: Ans[];
};
type Report = {
  surveyId: number;
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  status: number;
  summary: Summary;
  questionStatistics: QStat[];
  usersWhoFilled: UserS[];
  usersWhoDidNotFill: UserS[];
  userAnswerDetails: UserAns[];
};

export default function ReportDetailPage() {
  const params = useParams();
  const id = Number(params.surveyId);
  const [search, setSearch] = useState("");
  const q = new URLSearchParams();
  if (search.trim()) q.set("userSearch", search.trim());

  const { data, isLoading } = useQuery({
    queryKey: ["survey-report", id, search],
    queryFn: () =>
      api.get(`SurveyReports/${id}${q.toString() ? `?${q}` : ""}`).json<Report>(),
    enabled: Number.isFinite(id) && id > 0,
  });

  if (isLoading || !data)
    return (
      <Box>
        <LinearProgress />
        <Typography sx={{ p: 2 }}>Yükleniyor…</Typography>
      </Box>
    );

  return (
    <Box>
      <Button component={Link} href="/admin/reports" sx={{ mb: 2 }}>
        ← Liste
      </Button>
      <Typography variant="h5">{data.title}</Typography>
      <Typography color="text.secondary" sx={{ mb: 2 }}>
        {data.description}
      </Typography>
      <Paper sx={{ p: 2, mb: 2 }}>
        <Typography variant="subtitle1">Özet</Typography>
        <Typography sx={{ mb: 1 }}>
          Atanan: {data.summary.totalAssignedUsers} | Gönderen: {data.summary.totalSubmitted} | Bekleyen:{" "}
          {data.summary.totalPending} | Tamamlanma: {data.summary.completionRatePercent}%
        </Typography>
        <Box sx={{ width: "100%", height: 210 }}>
          <BarChart
            dataset={[
              { label: "Gönderilen", value: data.summary.totalSubmitted },
              { label: "Bekleyen", value: data.summary.totalPending },
            ]}
            xAxis={[{ scaleType: "band", dataKey: "label" }]}
            series={[{ dataKey: "value", label: "Kişi" }]}
            height={210}
            skipAnimation
          />
        </Box>
      </Paper>

      <Typography variant="h6" gutterBottom>
        Soru / şık dağılımı
      </Typography>
      {data.questionStatistics.map((qs) => (
        <Paper key={qs.questionId} sx={{ p: 2, mb: 2 }}>
          <Typography fontWeight="bold">
            {qs.sortOrder}. {qs.questionText}
          </Typography>
          <Box sx={{ width: "100%", height: 260, mt: 1 }}>
            <BarChart
              dataset={qs.optionCounts.map((o) => ({ label: o.optionText, value: o.responseCount }))}
              xAxis={[{ scaleType: "band", dataKey: "label" }]}
              series={[{ dataKey: "value", label: "Yanıt" }]}
              height={260}
              skipAnimation
            />
          </Box>
        </Paper>
      ))}

      <Divider sx={{ my: 3 }} />
      <TextField
        size="small"
        label="Kullanıcı adında ara"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        sx={{ mb: 2 }}
      />

      <Typography variant="h6">Dolduranlar</Typography>
      <Table size="small" sx={{ mb: 3 }}>
        <TableBody>
          {data.usersWhoFilled.map((u) => (
            <TableRow key={u.userId}>
              <TableCell>{u.fullName}</TableCell>
              <TableCell>{u.submittedAt && new Date(u.submittedAt).toLocaleString("tr")}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>

      <Typography variant="h6">Doldurmayanlar</Typography>
      <Table size="small" sx={{ mb: 3 }}>
        <TableBody>
          {data.usersWhoDidNotFill.map((u) => (
            <TableRow key={u.userId}>
              <TableCell>{u.fullName}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>

      <Typography variant="h6" sx={{ mt: 2 }}>
        Cevap detayı
      </Typography>

      <Stack spacing={1} sx={{ mt: 1 }}>
        {data.userAnswerDetails.map((u) => (
          <Accordion key={u.userId} disableGutters>
            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
              <Box>
                <Typography fontWeight="bold">{u.fullName}</Typography>
                <Typography variant="caption" color="text.secondary">
                  {new Date(u.submittedAt).toLocaleString("tr")}
                </Typography>
              </Box>
            </AccordionSummary>
            <AccordionDetails>
              <Table size="small">
                <TableHead>
                  <TableRow>
                    <TableCell>Soru</TableCell>
                    <TableCell>Cevap</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {u.answers.map((a) => (
                    <TableRow key={a.questionId}>
                      <TableCell>{a.questionText}</TableCell>
                      <TableCell>
                        <Chip size="small" label={a.selectedOptionText || "-"} />
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </AccordionDetails>
          </Accordion>
        ))}
      </Stack>
    </Box>
  );
}
