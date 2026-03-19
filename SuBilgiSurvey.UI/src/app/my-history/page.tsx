import { redirect } from "next/navigation";

export default function MyHistoryPage() {
  redirect("/my-surveys?tab=past");
}

