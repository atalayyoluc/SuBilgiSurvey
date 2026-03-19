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
  IconButton,
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

type Option = { sortOrder: number; optionText: string };
type Template = {
  id: number;
  name: string;
  options: { sortOrder: number; optionText: string }[];
};

export default function AnswerTemplatesPage() {
  const qc = useQueryClient();
  const [err, setErr] = useState<string | null>(null);
  const [open, setOpen] = useState(false);
  const [editing, setEditing] = useState<Template | null>(null);
  const [name, setName] = useState("");
  const [opts, setOpts] = useState<Option[]>([
    { sortOrder: 1, optionText: "" },
    { sortOrder: 2, optionText: "" },
  ]);

  const { data: list = [], isLoading } = useQuery({
    queryKey: ["answer-templates"],
    queryFn: () => api.get("AnswerTemplates").json<Template[]>(),
  });

  const save = useMutation({
    mutationFn: async () => {
      const valid = opts.filter((o) => o.optionText.trim());
      if (valid.length < 2 || valid.length > 4) throw new Error("2–4 şık girin");
      if (!name.trim()) throw new Error("Şablon adı gerekli");
      const body = {
        name: name.trim(),
        options: valid.map((o, i) => ({
          sortOrder: i + 1,
          optionText: o.optionText.trim(),
        })),
      };
      if (editing) {
        await api.put(`AnswerTemplates/${editing.id}`, {
          json: { id: editing.id, ...body },
        });
      } else {
        await api.post("AnswerTemplates", { json: body });
      }
    },
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["answer-templates"] });
      setOpen(false);
      setErr(null);
    },
    onError: async (e) => setErr(await readProblem(e)),
  });

  const del = useMutation({
    mutationFn: (id: number) => api.delete(`AnswerTemplates/${id}`),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["answer-templates"] }),
    onError: async (e) => setErr(await readProblem(e)),
  });

  const openCreate = () => {
    setEditing(null);
    setName("");
    setOpts([
      { sortOrder: 1, optionText: "" },
      { sortOrder: 2, optionText: "" },
    ]);
    setOpen(true);
  };

  const fixOpenEdit = (t: Template) => {
    setEditing(t);
    setName(t.name);
    setOpts(
      t.options.length >= 2
        ? t.options.map((o, i) => ({
            sortOrder: i + 1,
            optionText: o.optionText,
          }))
        : [
            { sortOrder: 1, optionText: "" },
            { sortOrder: 2, optionText: "" },
          ],
    );
    setOpen(true);
  };

  return (
    <Box>
      <Box sx={{ display: "flex", justifyContent: "space-between", mb: 2 }}>
        <Typography variant="h5">Cevap şablonları</Typography>
        <Button startIcon={<AddIcon />} variant="contained" onClick={openCreate}>
          Yeni
        </Button>
      </Box>
      {err && (
        <Alert severity="error" onClose={() => setErr(null)} sx={{ mb: 2 }}>
          {err}
        </Alert>
      )}
      {isLoading ? (
        <Typography>Yükleniyor…</Typography>
      ) : (
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Ad</TableCell>
              <TableCell>Şık sayısı</TableCell>
              <TableCell align="right">İşlem</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {list.map((t) => (
              <TableRow key={t.id}>
                <TableCell>{t.name}</TableCell>
                <TableCell>{t.options?.length ?? 0}</TableCell>
                <TableCell align="right">
                  <IconButton size="small" onClick={() => fixOpenEdit(t)}>
                    <EditIcon />
                  </IconButton>
                  <IconButton
                    size="small"
                    color="error"
                    onClick={() => {
                      if (confirm("Silinsin mi?")) del.mutate(t.id);
                    }}
                  >
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      )}

      <Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth="sm">
        <DialogTitle>{editing ? "Şablon güncelle" : "Yeni şablon"}</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            label="Şablon adı"
            value={name}
            onChange={(e) => setName(e.target.value)}
            margin="normal"
          />
          <Typography variant="subtitle2" sx={{ mt: 2 }}>
            Şıklar (2–4)
          </Typography>
          {opts.map((o, i) => (
            <TextField
              key={i}
              fullWidth
              label={`Şık ${i + 1}`}
              value={o.optionText}
              onChange={(e) => {
                const n = [...opts];
                n[i] = { ...n[i], optionText: e.target.value };
                setOpts(n);
              }}
              margin="dense"
            />
          ))}
          <Box sx={{ mt: 1 }}>
            {opts.length < 4 && (
              <Button
                size="small"
                onClick={() =>
                  setOpts([...opts, { sortOrder: opts.length + 1, optionText: "" }])
                }
              >
                Şık ekle
              </Button>
            )}
            {opts.length > 2 && (
              <Button size="small" color="warning" onClick={() => setOpts(opts.slice(0, -1))}>
                Son şıkkı kaldır
              </Button>
            )}
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
