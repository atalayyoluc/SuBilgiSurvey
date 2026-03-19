"use client";

import { useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { Alert, Box, LinearProgress, Paper, Typography } from "@mui/material";
import { api, readProblem } from "@/shared/api/client";

type Q = {
  questionId: number;
  questionText: string;
  answerTemplateOptionId: number;
  optionText: string;
};

type Detail = {
  surveyId: number;
  title: string;
  description: string;
  submittedAt: string;
  questions: Q[];
};

export default function MyHistoryDetailPage() {
  const params = useParams();
  const id = Number(params.surveyId);

  const { data, isLoading, error } = useQuery({
    queryKey: ["my-history", id],
    queryFn: async () => {
      try {
        return await api.get(`SurveyFilling/my-history/${id}`).json<Detail>();
      } catch (e) {
        throw new Error(await readProblem(e));
      }
    },
    enabled: id > 0,
  });

  if (isLoading || !data)
    return (
      <Box>
        <LinearProgress />
        <Typography sx={{ p: 2 }}>Yükleniyor…</Typography>
      </Box>
    );

  return (
    <Box sx={{ maxWidth: 720 }}>
      {error instanceof Error && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error.message}
        </Alert>
      )}
      <Typography variant="h5" gutterBottom>
        {data.title}
      </Typography>
      <Typography color="text.secondary" sx={{ mb: 1 }}>
        {data.description}
      </Typography>
      <Typography variant="caption" display="block" sx={{ mb: 3 }}>
        Gönderim: {new Date(data.submittedAt).toLocaleString("tr")}
      </Typography>

      {data.questions.map((q, i) => (
        <Paper key={q.questionId} sx={{ p: 2, mb: 2 }}>
          <Typography variant="subtitle1" gutterBottom>
            {i + 1}. {q.questionText}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Cevabınız: {q.optionText || "(bulunamadı)"}
          </Typography>
        </Paper>
      ))}
    </Box>
  );
}

