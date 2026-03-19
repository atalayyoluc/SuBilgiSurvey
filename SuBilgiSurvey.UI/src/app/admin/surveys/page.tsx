"use client";

import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useState, useMemo } from "react";
import {
  Box,
  Button,
  Paper,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  FormControlLabel,
  Checkbox,
  IconButton,
  InputLabel,
  MenuItem,
  Select,
  TableContainer,
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  TextField,
  Typography,
  Alert,
  Stack,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";
import { api, readProblem } from "@/shared/api/client";

type Survey = {
  id: number;
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  status: number;
  questionIds: number[];
  assignedUserIds: number[];
};
type Q = { id: number; text: string };
type UserRow = { id: number; fullName: string; email: string; role: number };

export default function SurveysPage() {
  const qc = useQueryClient();
  const [err, setErr] = useState<string | null>(null);
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Survey | null>(null);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [status, setStatus] = useState(1);
  const [filterStatus, setFilterStatus] = useState<string>("");
  const [filterFromDate, setFilterFromDate] = useState<string>("");
  const [filterToDate, setFilterToDate] = useState<string>("");
  const [selQuestions, setSelQuestions] = useState<number[]>([]);
  const [selUsers, setSelUsers] = useState<number[]>([]);

  const filterParams = new URLSearchParams();
  if (filterStatus !== "") filterParams.set("status", filterStatus);
  // StartDate <= filterTo AND EndDate >= filterFrom
  if (filterFromDate) filterParams.set("endDateOnOrAfter", new Date(filterFromDate).toISOString());
  if (filterToDate) filterParams.set("startDateOnOrBefore", new Date(filterToDate).toISOString());

  const { data: surveys = [] } = useQuery({
    queryKey: ["surveys", filterStatus, filterFromDate, filterToDate],
    queryFn: () =>
      api
        .get(`Surveys${filterParams.toString() ? `?${filterParams}` : ""}`)
        .json<Survey[]>(),
  });
  const { data: questions = [] } = useQuery({
    queryKey: ["questions"],
    queryFn: () => api.get("Questions").json<Q[]>(),
  });
  const { data: users = [] } = useQuery({
    queryKey: ["users"],
    queryFn: () => api.get("Users").json<UserRow[]>(),
  });

  const regularUsers = useMemo(() => users.filter((u) => u.role === 0), [users]);

  const resetForm = (s?: Survey | null) => {
    if (s) {
      setEditing(s);
      setTitle(s.title);
      setDescription(s.description);
      setStartDate(s.startDate.slice(0, 16));
      setEndDate(s.endDate.slice(0, 16));
      setStatus(s.status);
      setSelQuestions([...s.questionIds]);
      setSelUsers([...s.assignedUserIds]);
    } else {
      setEditing(null);
      setTitle("");
      setDescription("");
      const now = new Date();
      const week = new Date(now.getTime() + 7 * 86400000);
      setStartDate(now.toISOString().slice(0, 16));
      setEndDate(week.toISOString().slice(0, 16));
      setStatus(1);
      setSelQuestions([]);
      setSelUsers([]);
    }
  };

  const save = useMutation({
    mutationFn: async () => {
      if (!title.trim()) throw new Error("Başlık gerekli");
      if (selQuestions.length === 0) throw new Error("En az bir soru seçin");
      if (selUsers.length === 0) throw new Error("En az bir kullanıcı atayın");
      const body = {
        title: title.trim(),
        description: description.trim(),
        startDate: new Date(startDate).toISOString(),
        endDate: new Date(endDate).toISOString(),
        status,
        questionIds: selQuestions,
        assignedUserIds: selUsers,
      };
      if (editing) {
        await api.put(`Surveys/${editing.id}`, {
          json: { id: editing.id, ...body },
        });
      } else {
        await api.post("Surveys", { json: body });
      }
    },
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["surveys"] });
      setOpen(false);
      setErr(null);
    },
    onError: async (e) => setErr(await readProblem(e)),
  });

  const del = useMutation({
    mutationFn: (id: number) => api.delete(`Surveys/${id}`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["surveys"] }),
    onError: async (e) => setErr(await readProblem(e)),
  });

  const toggle = (id: number, list: number[], setList: (v: number[]) => void) => {
    if (list.includes(id)) setList(list.filter((x) => x !== id));
    else setList([...list, id]);
  };

  return (
    <Box>
      <Box sx={{ display: "flex", justifyContent: "space-between", mb: 2 }}>
        <Typography variant="h5">Anketler</Typography>
        <Button
          startIcon={<AddIcon />}
          variant="contained"
          onClick={() => {
            resetForm(null);
            setOpen(true);
          }}
        >
          Yeni anket
        </Button>
      </Box>
      <Paper sx={{ p: 2, mb: 2 }}>
        <Stack direction="row" spacing={2} alignItems="flex-end" flexWrap="wrap">
          <FormControl size="small" sx={{ minWidth: 160 }}>
            <InputLabel>Durum</InputLabel>
            <Select
              label="Durum"
              value={filterStatus}
              onChange={(e) => setFilterStatus(e.target.value as string)}
            >
              <MenuItem value="">Tümü</MenuItem>
              <MenuItem value="1">Aktif</MenuItem>
              <MenuItem value="0">Pasif</MenuItem>
            </Select>
          </FormControl>
          <TextField
            size="small"
            label="Başlangıç"
            type="date"
            value={filterFromDate}
            onChange={(e) => setFilterFromDate(e.target.value)}
            InputLabelProps={{ shrink: true }}
          />
          <TextField
            size="small"
            label="Bitiş"
            type="date"
            value={filterToDate}
            onChange={(e) => setFilterToDate(e.target.value)}
            InputLabelProps={{ shrink: true }}
          />
          <Button
            size="small"
            variant="outlined"
            onClick={() => {
              setFilterStatus("");
              setFilterFromDate("");
              setFilterToDate("");
            }}
          >
            Temizle
          </Button>
        </Stack>
      </Paper>
      {err && (
        <Alert severity="error" onClose={() => setErr(null)} sx={{ mb: 2 }}>
          {err}
        </Alert>
      )}
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
              <TableCell>Durum</TableCell>
              <TableCell>Başlangıç</TableCell>
              <TableCell>Bitiş</TableCell>
              <TableCell align="right">İşlem</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {surveys.map((s) => (
              <TableRow key={s.id}>
                <TableCell>{s.title}</TableCell>
                <TableCell>{s.status === 1 ? "Aktif" : "Pasif"}</TableCell>
                <TableCell>{new Date(s.startDate).toLocaleString("tr")}</TableCell>
                <TableCell>{new Date(s.endDate).toLocaleString("tr")}</TableCell>
                <TableCell align="right">
                  <IconButton size="small" onClick={() => { resetForm(s); setOpen(true); }}>
                    <EditIcon />
                  </IconButton>
                  <IconButton
                    size="small"
                    color="error"
                    onClick={() => confirm("Silinsin mi?") && del.mutate(s.id)}
                  >
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth="md">
        <DialogTitle>{editing ? "Anket güncelle" : "Yeni anket"}</DialogTitle>
        <DialogContent>
          <TextField fullWidth label="Başlık" value={title} onChange={(e) => setTitle(e.target.value)} margin="normal" />
          <TextField
            fullWidth
            label="Açıklama"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            margin="normal"
            multiline
            minRows={2}
          />
          <Stack direction="row" spacing={2} sx={{ mt: 1 }}>
            <TextField
              label="Başlangıç"
              type="datetime-local"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              InputLabelProps={{ shrink: true }}
              fullWidth
            />
            <TextField
              label="Bitiş"
              type="datetime-local"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              InputLabelProps={{ shrink: true }}
              fullWidth
            />
          </Stack>
          <FormControl fullWidth margin="normal">
            <InputLabel>Durum</InputLabel>
            <Select value={status} label="Durum" onChange={(e) => setStatus(Number(e.target.value))}>
              <MenuItem value={1}>Aktif</MenuItem>
              <MenuItem value={0}>Pasif</MenuItem>
            </Select>
          </FormControl>
          <Typography variant="subtitle2" sx={{ mt: 2 }}>
            Sorular
          </Typography>
          <Box sx={{ maxHeight: 160, overflow: "auto", border: 1, borderColor: "divider", p: 1 }}>
            {questions.map((q) => (
              <FormControlLabel
                key={q.id}
                control={
                  <Checkbox
                    checked={selQuestions.includes(q.id)}
                    onChange={() => toggle(q.id, selQuestions, setSelQuestions)}
                  />
                }
                label={`#${q.id} ${q.text.slice(0, 80)}${q.text.length > 80 ? "…" : ""}`}
              />
            ))}
          </Box>
          <Typography variant="subtitle2" sx={{ mt: 2 }}>
            Atanan kullanıcılar
          </Typography>
          <Box sx={{ maxHeight: 160, overflow: "auto", border: 1, borderColor: "divider", p: 1 }}>
            {regularUsers.map((u) => (
              <FormControlLabel
                key={u.id}
                control={
                  <Checkbox
                    checked={selUsers.includes(u.id)}
                    onChange={() => toggle(u.id, selUsers, setSelUsers)}
                  />
                }
                label={`${u.fullName} (${u.email})`}
              />
            ))}
          </Box>
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
