"use client";

import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Button } from "../ui/button";
import { useState } from "react";

type LoginDialogProps = {
  open: boolean;
  setOpenAction: (open: boolean) => void;
};

export default function LoginDialog({
  open,
  setOpenAction: setOpen,
}: LoginDialogProps) {
  //const [open, setOpen] = useState(false);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const res = await fetch(
        `${process.env.NEXT_PUBLIC_USERS_API_BASE_URL}/api/User/login`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ Email: email, Password: password }),
        }
      );

      if (!res.ok) {
        // Try to parse error message from the server, fall back to status text
        let msg = res.statusText;
        try {
          const json = await res.json();

          msg = json?.message || msg;
        } catch (err) {
          /* ignore JSON parse errors */
        }
        setError(msg || "Login failed");
        return;
      }
      // Server returns the token as plain text in the response body.
      const token = await res.text();

      if (!token) {
        setError("Login failed: no token returned");
        return;
      }

      try {
        localStorage.setItem("accessToken", token);
        // notify same-tab listeners that auth state changed
        window.dispatchEvent(new Event("authChanged"));
      } catch (err) {
        // ignore storage errors
        console.warn("Unable to store token in localStorage", err);
      }

      // Success: close dialog. Caller can handle routing / further auth handling.
      setOpen(false);
      /* eslint-disable  @typescript-eslint/no-explicit-any */
    } catch (err: any) {
      setError(err?.message || "Network error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="p-10">
        <DialogHeader>
          <DialogTitle className="text-primary-gray text-3xl font-bold font-['Poppins']">
            Welcome back
          </DialogTitle>
          <DialogDescription>
            {/* Enter your email below to login to your account */}
          </DialogDescription>
        </DialogHeader>

        <form className="grid gap-4" onSubmit={handleSubmit}>
          <div className="grid gap-2">
            <Label
              htmlFor="email"
              className="text-xs text-muted-gray font-bold"
            >
              Email
            </Label>
            <Input
              id="email"
              type="email"
              placeholder="m@example.com"
              required
              className="h-12"
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>
          <div className="grid gap-2">
            <Label
              htmlFor="password"
              className="text-xs text-muted-gray font-bold"
            >
              Password
            </Label>
            <Input
              id="password"
              type="password"
              placeholder="••••••••"
              required
              className="h-12"
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>

          {error && (
            <p className="text-sm text-danger mt-4" role="alert">
              {error}
            </p>
          )}
          <Button
            type="submit"
            className="h-9 bg-primary-green font-bold cursor-pointer"
            disabled={loading}
          >
            {loading ? "Logging in..." : "Log In"}
          </Button>
        </form>

        <a
          href="#"
          className="text-xs font-bold text-primary-green justify-self-end underline-offset-4 hover:underline"
        >
          Forgot password?
        </a>

        <Button variant="outline" className="h-9 cursor-pointer">
          Sign Up
        </Button>
      </DialogContent>
    </Dialog>
  );
}
