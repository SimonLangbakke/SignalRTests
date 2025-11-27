import * as React from "react";
import { cva, type VariantProps } from "class-variance-authority";
import { cn } from "@/lib/utils";

const badgeVariants = cva(
  "inline-flex items-center rounded px-3 py-0.5 text-sm font-semibold",
  {
    variants: {
      variant: {
        success: "bg-green-200 text-green-800",
        warning: "bg-yellow-400 text-black",
        info: "bg-blue-500 text-white",
        outline: "border border-gray-400 text-gray-700",
      },
    },
    defaultVariants: { variant: "info" },
  }
)

export interface BadgeProps
  extends React.HTMLAttributes<HTMLDivElement>,
    VariantProps<typeof badgeVariants> {}

function Badge({ className, variant, ...props }: BadgeProps) {
  return (
    <div className={cn(badgeVariants({ variant }), className)} {...props} />
  );
}

export { Badge, badgeVariants };
