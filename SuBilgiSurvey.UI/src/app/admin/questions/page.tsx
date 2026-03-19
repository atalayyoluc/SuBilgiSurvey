"use client";

import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useState } from "react";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  IconButton,
  InputLabel,
  MenuItem,
  Select,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  TextField,
  Typography,
  Alert,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";
import { api, readProblem } from "@/shared/api/client";

type Q = { id: number; text: string; answerTemplateId: number };
type Tpl = { id: number; name: string };

export default function QuestionsPage() {
  const qc = useQueryClient();
  const [err, setErr] = useState<string | null>(null);
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Q | null>(null);
  const [text, setText] = useState("");
  const [templateId, setTemplateId] = useState<number>(0);

  const { data: questions = [] } = useQuery({
    queryKey: ["questions"],
    queryFn: () => api.get("Questions").json<Q[]>(),
  });
  const { data: templates = [] } = useQuery({
    queryKey: ["answer-templates"],
    queryFn: () => api.get("AnswerTemplates").json<Tpl[]>(),
  });

  const save = useMutation({
    mutationFn: async () => {
      if (!text.trim()) throw new Error("Soru metni gerekli");
      if (!templateId) throw new Error("Şablon seçin");
      if (editing) {
        await api.put(`Questions/${editing.id}`, {
          json: { id: editing.id, text: text.trim(), answerTemplateId: templateId },
        });
      } else {
        await api.post("Questions", {
          json: { text: text.trim(), answerTemplateId: templateId },
        });
      }
    },
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["questions"] });
      setOpen(false);
      setErr(null);
    },
    onError: async (e) => setErr(await readProblem(e)),
  });

  const del = useMutation({
    mutationFn: (id: number) => api.delete(`Questions/${id}`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["questions"] }),
    onError: async (e) => setErr(await readProblem(e)),
  });

  return (
    <Box>
      <Box sx={{ display: "flex", justifyContent: "space-between", mb: 2 }}>
        <Typography variant="h5">Sorular</Typography>
        <Button
          startIcon={<AddIcon />}
          variant="contained"
          onClick={() => {
            setEditing(null);
            setText("");
            setTemplateId(templates[0]?.id ?? 0);
            setOpen(true);
          }}
        >
          Yeni
        </Button>
      </Box>
      {err && (
        <Alert severity="error" onClose={() => setErr(null)} sx={{ mb: 2 }}>
          {err}
        </Alert>
      )}
      <Table size="small">
        <TableHead>
          <TableRow>
            <TableCell>Metin</TableCell>
            <TableCell>Şablon ID</TableCell>
            <TableCell align="right">İşlem</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {questions.map((q) => (
            <TableRow key={q.id}>
              <TableCell>{q.text}</TableCell>
              <TableCell>{q.answerTemplateId}</TableCell>
              <TableCell align="right">
                <IconButton
                  size="small"
                  onClick={() => {
                    setEditing(q);
                    setText(q.text);
                    setTemplateId(q.answerTemplateId);
                    setOpen(true);
                  }}
                >
                  <EditIcon />
                </IconButton>
                <IconButton
                  size="small"
                  color="error"
                  onClick={() => confirm("Silinsin mi?") && del.mutate(q.id)}
                >
                  <DeleteIcon />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>

      <Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth="sm">
        <DialogTitle>{editing ? "Soru güncelle" : "Yeni soru"}</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            label="Soru metni"
            value={text}
            onChange={(e) => setText(e.target.value)}
            margin="normal"
            multiline
            minRows={2}
          />
          <FormControl fullWidth margin="normal">
            <InputLabel>Cevap şablonu</InputLabel>
            <Select
              value={templateId || ""}
              label="Cevap şablonu"
              onChange={(e) => setTemplateId(Number(e.target.value))}
            >
              {templates.map((t) => (
                <MenuItem key={t.id} value={t.id}>
                  {t.name} (#{t.id})
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpen(false)}>İptal</Button>
          <Button variant="contained" onClick={() => save.mutate()} disabled={save.isPending}>
            Kaydet
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}
