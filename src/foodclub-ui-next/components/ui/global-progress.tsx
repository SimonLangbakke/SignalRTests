"use client";

import { useEffect, useRef, useState } from "react";
import { usePathname, useSearchParams } from "next/navigation";

export default function GlobalProgress() {
  const pathname = usePathname();
  const searchParams = useSearchParams();
  const lastPathnameRef = useRef<string | null>(null);
  const lastSearchRef = useRef<string | null>(null);
  const [progress, setProgress] = useState(0);
  const [visible, setVisible] = useState(false);
  const timerRef = useRef<number | null>(null);
  const activeCountRef = useRef(0);

  const start = () => {
    activeCountRef.current += 1;
    if (!visible) {
      setVisible(true);
      setProgress(10);
    }
    if (!timerRef.current) {
      timerRef.current = window.setInterval(() => {
        setProgress((p) => {
          const increment = Math.max(1, Math.floor(Math.random() * 5));
          const next = Math.min(p + increment, 90);
          return next;
        });
      }, 400);
    }
  };

  const done = () => {
    activeCountRef.current = Math.max(0, activeCountRef.current - 1);
    if (activeCountRef.current > 0) return;
    setProgress(100);
    if (timerRef.current) {
      window.clearInterval(timerRef.current);
      timerRef.current = null;
    }
    setTimeout(() => {
      setVisible(false);
      setProgress(0);
    }, 350);
  };

  useEffect(() => {
    const onStart = () => start();
    const onEnd = () => done();

    window.addEventListener("app:fetch-start", onStart as EventListener);
    window.addEventListener("app:fetch-end", onEnd as EventListener);

    return () => {
      window.removeEventListener("app:fetch-start", onStart as EventListener);
      window.removeEventListener("app:fetch-end", onEnd as EventListener);
    };
  }, []);

  useEffect(() => {
    const current = pathname || null;
    const last = lastPathnameRef.current;
    const search = (searchParams && searchParams.toString()) || null;
    const lastSearch = lastSearchRef.current;
    if (last === null) {
      lastPathnameRef.current = current;
      lastSearchRef.current = search;
      return;
    }
    if (current !== last || search !== lastSearch) {
      start();
      setTimeout(() => done(), 600);
      lastPathnameRef.current = current;
      lastSearchRef.current = search;
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pathname, searchParams?.toString()]);

  if (!visible) return null;

  return (
    <div className="fixed left-0 right-0 top-0 z-50 pointer-events-none">
      <div className="h-1 bg-emerald-100 w-full">
        <div
          className="h-1 bg-emerald-600 transition-all ease-linear duration-200"
          style={{ width: `${progress}%` }}
        />
      </div>
    </div>
  );
}
