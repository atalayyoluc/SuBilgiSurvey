"use client";

import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useParams, useRouter } from "next/navigation";
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  RadioGroup,
  FormControlLabel,
  Radio,
  Typography,
  Paper,
  Alert,
  LinearProgress,
} from "@mui/material";
import { api, readProblem } from "@/shared/api/client";
import { useState } from "react";

type Opt = { answerTemplateOptionId: number; optionText: string; sortOrder: number };
type Q = { questionId: number; questionText: string; options: Opt[] };
type SurveyFill = {
  surveyId: number;
  title: string;
  description: string;
  questions: Q[];
};

export default function FillSurveyPage() {
  const params = useParams();
  const router = useRouter();
  const qc = useQueryClient();
  const id = Number(params.surveyId);
  const [answers, setAnswers] = useState<Record<number, number>>({});
  const [err, setErr] = useState<string | null>(null);

  const { data, isLoading } = useQuery({
    queryKey: ["survey-fill", id],
    queryFn: () => api.get(`SurveyFilling/${id}`).json<SurveyFill>(),
    enabled: id > 0,
  });

  const submit = useMutation({
    mutationFn: async () => {
      if (!data) return;
      const list = data.questions.map((q) => ({
        questionId: q.questionId,
        answerTemplateOptionId: answers[q.questionId],
      }));
      if (list.some((x) => !x.answerTemplateOptionId)) {
        throw new Error("Tüm soruları yanıtlayın");
      }
      await api.post("SurveyFilling/submit", {
        json: { surveyId: id, answers: list },
      });
    },
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["my-surveys"] });
      router.push("/my-surveys");
    },
    onError: async (e) => setErr(await readProblem(e)),
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
      <Typography variant="h5" gutterBottom>
        {data.title}
      </Typography>
      <Typography color="text.secondary" sx={{ mb: 3 }}>
        {data.description}
      </Typography>
      {err && (
        <Alert severity="error" sx={{ mb: 2 }} onClose={() => setErr(null)}>
          {err}
        </Alert>
      )}
      {data.questions.map((q, i) => (
        <Paper key={q.questionId} sx={{ p: 2, mb: 2 }}>
          <Typography variant="subtitle1" gutterBottom>
            {i + 1}. {q.questionText}
          </Typography>
          <FormControl component="fieldset">
            <FormLabel component="legend">Seçiminiz</FormLabel>
            <RadioGroup
              value={answers[q.questionId] ?? ""}
              onChange={(_, v) =>
                setAnswers((a) => ({ ...a, [q.questionId]: Number(v) }))
              }
            >
              {q.options
                .slice()
                .sort((a, b) => a.sortOrder - b.sortOrder)
                .map((o) => (
                  <FormControlLabel
                    key={o.answerTemplateOptionId}
                    value={o.answerTemplateOptionId}
                    control={<Radio />}
                    label={o.optionText}
                  />
                ))}
            </RadioGroup>
          </FormControl>
        </Paper>
      ))}
      <Button variant="contained" size="large" onClick={() => submit.mutate()} disabled={submit.isPending}>
        Gönder
      </Button>
    </Box>
  );
}
