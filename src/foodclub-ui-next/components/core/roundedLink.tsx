"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";


export default function RoundedLink({
    children,
    href,
    isActive,
    isDisabled,
}: Readonly<{
    children: React.ReactNode;
    href: string,
    isActive?: boolean,
    isDisabled?: boolean
}>) {
    const pathname = usePathname();
    const computedIsActive = isActive !== undefined ? isActive : pathname === href;

    return (
        // bg-green-light - active, bg-gray-light - inActive
        <Link
            href={href}
            className={`p-2 ${isDisabled && "pointer-events-none opacity-50 cursor-not-allowed"} ${computedIsActive ? "bg-green-light" : "bg-gray-light hover:bg-muted hover:text-foreground"} rounded-[50%] px-5 py-2 text-primary-green text-xs font-bold font-['Poppins'] transition-colors`}
        >
            {children}
        </Link>
    );
}