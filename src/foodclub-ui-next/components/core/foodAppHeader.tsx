"use client";

import Link from "next/link";
import Image from "next/image";
import { useEffect, useState } from "react";
import LoginDialog from "./loginDialog";
import SignUpDialog from "./signUpDialog";
import { jwtDecode } from "jwt-decode";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import GlobalProgress from "@/components/ui/global-progress";

export default function FoodAppHeader() {
  const [openAuthDialog, setOpenAuthDialog] = useState(false);
  const [openSignUpDialog, setOpenSignUpDialog] = useState(false);

  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);
  const router = useRouter();

  useEffect(() => {
    checkToken();

    const onStorage = (e: StorageEvent) => {
      if (e.key === "accessToken") checkToken();
    };

    const onAuthChanged = () => checkToken();

    window.addEventListener("storage", onStorage);
    window.addEventListener("authChanged", onAuthChanged);

    return () => {
      window.removeEventListener("storage", onStorage);
      window.removeEventListener("authChanged", onAuthChanged);
    };
  }, []);

  function handleLogout() {
    try {
      localStorage.removeItem("accessToken");
    } catch (err) {
      /* ignore */
    }
    setIsAuthenticated(false);
    // notify other listeners (same-tab)
    window.dispatchEvent(new Event("authChanged"));
    router.replace("/");
  }

  function checkToken() {
    const token = localStorage.getItem("accessToken");
    if (token) {
      /* eslint-disable  @typescript-eslint/no-explicit-any */
      const decoded: any = jwtDecode(token);
      setUser(decoded.unique_name);

      setIsAuthenticated(true);
    }
  }

  return (
    <>
      {/* TODO: move this to a client component */}
      <LoginDialog open={openAuthDialog} setOpenAction={setOpenAuthDialog} />
      <SignUpDialog
        open={openSignUpDialog}
        setOpenAction={setOpenSignUpDialog}
      />

      <header className="relative flex justify-evenly items-center gap-5 h-20 outline-1 outline-gray-200">
        <GlobalProgress />
        <div className="flex h-20 gap-4 items-center">
          <Link
            href="/"
            className="text-primary-gray text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
          >
            <Image
              src="/source_logo_plateful.png"
              alt="Logo"
              width={73}
              height={73}
            />
          </Link>
          <Link
            href="/"
            className="py-4 px-1 text-primary-green text-xl font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
          >
            Plateful DK
          </Link>
        </div>

        <div className="flex h-14 gap-10">
          <Link
            href="/discoverEvents"
            className="py-4 px-1 text-primary-gray text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
          >
            Discover
          </Link>
          {isAuthenticated && (
            <Link
              href="/createFoodEvent"
              className="py-4 px-1 text-primary-gray text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
            >
              Host a Meal
            </Link>
          )}

          <Link
            href="#"
            className="py-4 px-1 text-primary-gray text-base font-bold font-['Poppins'] hover:bg-muted hover:text-foreground transition-colors"
          >
            How it Works
          </Link>
        </div>

        {isAuthenticated ? (
          <div className="flex gap-4">
            <span className="py-2 px-8 text-primary-gray text-base font-bold font-['Poppins']">
              <Link href="/userProfile">{user}</Link>
            </span>
            <Button
              variant="outline"
              onClick={handleLogout}
              className="border-emerald-800 text-emerald-800 hover:bg-emerald-50"
            >
              Sign out
            </Button>
          </div>
        ) : (
          <div className="flex gap-4">
            <Button
              variant="outline"
              onClick={() => setOpenAuthDialog(true)}
              className="border-emerald-800 text-emerald-800 hover:bg-emerald-50"
            >
              Log in
            </Button>
            <Button
              variant="default"
              onClick={() => setOpenSignUpDialog(true)}
              className="bg-emerald-800 hover:bg-emerald-700"
            >
              Sign up
            </Button>
          </div>
        )}
      </header>
    </>
  );
}
