"use client";

import { useState, FormEvent } from "react";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { createUserOnSignup } from "@/services/api/user-api.service";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { Button } from "../ui/button";

type SignUpDialogProps = {
  open: boolean;
  setOpenAction: (open: boolean) => void;
};

export default function SignUpDialog({
  open,
  setOpenAction: setOpen,
}: SignUpDialogProps) {
  // form state
  const [month, setMonth] = useState<string>("");
  const [day, setDay] = useState<string>("");
  const [year, setYear] = useState<string>("");
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  function daysInMonth(y: number, m: number) {
    // y = full year, m = 1-12
    return new Date(y, m, 0).getDate();
  }

  function calculateAgeFromBirth(birth: Date) {
    const today = new Date();
    let age = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) {
      age--;
    }
    return age;
  }

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);

    const form = e.currentTarget;
    const fd = new FormData(form);
    const fullName = String(fd.get("fullName") || "").trim();
    const email = String(fd.get("email") || "").trim();
    const password = String(fd.get("password") || "");
    const confirmPassword = String(fd.get("confirmPassword") || "");

    if (!month || !day || !year) {
      setError("Please select your full birthday.");
      return;
    }

    const birth = new Date(Number(year), Number(month) - 1, Number(day));
    const age = calculateAgeFromBirth(birth);
    if (age < 18) {
      setError("You must be at least 18 years old to sign up.");
      return;
    }

    if (password !== confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    setSubmitting(true);
    try {
      const yyyy = String(year);
      const mm = String(Number(month)).padStart(2, "0");
      const dd = String(Number(day)).padStart(2, "0");
      const birthdayDateOnly = `${yyyy}-${mm}-${dd}`;

      const payload = {
        name: fullName,
        email,
        password,
        birthday: birthdayDateOnly,
      };

      const created = await createUserOnSignup(payload);
      if (!created) {
        setError("Sign up failed. Please try again.");
        return;
      }
      // success — close dialog (or show success UI)
      setOpen(false);
    } catch (err) {
      console.error(err);
      setError("Failed to sign up. Please try again.");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogContent className="p-10">
        <DialogHeader>
          <DialogTitle className="text-primary-gray text-3xl font-bold font-['Poppins']">
            Create your account
          </DialogTitle>
          <DialogDescription>To join dinners or host your own.</DialogDescription>
        </DialogHeader>

        <form
          className="grid gap-4"
          onSubmit={handleSubmit}
          aria-busy={submitting}
        >
          <div className="grid gap-2">
            <Label
              htmlFor="fullName"
              className="text-xs text-muted-gray font-bold"
            >
              Full Name
            </Label>
            <Input
              id="fullName"
              name="fullName"
              type="text"
              placeholder="Enter your full name"
              required
              className="h-12"
            />
          </div>

          <div className="grid gap-2">
            <Label
              htmlFor="email"
              className="text-xs text-muted-gray font-bold"
            >
              Email
            </Label>
            <Input
              id="email"
              name="email"
              type="email"
              placeholder="m@example.com"
              required
              className="h-12"
            />
          </div>

          {/* Birthday: month / day / year dropdowns - only allow 18+ */}
          <div className="grid gap-2">
            <Label
              htmlFor="birthday"
              className="text-xs text-muted-gray font-bold"
            >
              Birthday
            </Label>

            <div className="flex gap-2">
              <select
                aria-label="Month"
                name="month"
                value={month}
                onChange={(e) => setMonth(e.target.value)}
                className="h-12 px-3 w-36"
                id="month-select"
              >
                <option value="" disabled>
                  Month
                </option>
                {[
                  "January",
                  "February",
                  "March",
                  "April",
                  "May",
                  "June",
                  "July",
                  "August",
                  "September",
                  "October",
                  "November",
                  "December",
                ].map((m, i) => (
                  <option key={m} value={String(i + 1)}>
                    {m}
                  </option>
                ))}
              </select>

              <select
                aria-label="Day"
                name="day"
                value={day}
                onChange={(e) => setDay(e.target.value)}
                className="h-12 px-3 w-24"
                id="day-select"
              >
                <option value="" disabled>
                  Day
                </option>
                {Array.from({
                  length: daysInMonth(Number(year) || 2000, Number(month) || 1),
                }).map((_, i) => (
                  <option key={i + 1} value={String(i + 1)}>
                    {i + 1}
                  </option>
                ))}
              </select>

              <select
                aria-label="Year"
                name="year"
                value={year}
                onChange={(e) => setYear(e.target.value)}
                className="h-12 px-3 w-32"
                id="year-select"
              >
                <option value="" disabled>
                  Year
                </option>
                {Array.from({ length: 83 }).map((_, i) => {
                  const y = new Date().getFullYear() - 18 - i; // up to ~100 years back
                  return (
                    <option key={y} value={String(y)}>
                      {y}
                    </option>
                  );
                })}
              </select>
            </div>

            {error ? (
              <p className="text-xs text-rose-600 mt-1" id="birthday-error">
                {error}
              </p>
            ) : null}
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
              name="password"
              type="password"
              placeholder="8+ characters"
              required
              className="h-12"
            />
          </div>

          <div className="grid gap-2">
            <Label
              htmlFor="confirmPassword"
              className="text-xs text-muted-gray font-bold"
            >
              Confirm password
            </Label>
            <Input
              id="confirmPassword"
              name="confirmPassword"
              type="password"
              placeholder="Repeat password"
              required
              className="h-12"
            />
          </div>

          <Button
            type="submit"
            className="h-9 bg-primary-green font-bold cursor-pointer"
            disabled={submitting}
            aria-busy={submitting}
          >
            {submitting ? "Signing up…" : "Sign Up"}
          </Button>
        </form>

        <p className="justify-self-center text-xs text-muted-gray">
          By continuing, you agree to our Terms and Privacy.
        </p>

        <a
          href="#"
          className="text-xs font-bold text-primary-green justify-self-end underline-offset-4 hover:underline"
        >
          Already have an account? Log in
        </a>
      </DialogContent>
    </Dialog>
  );
}
