import { cn } from "@/lib/utils";

/**
 * Creates a vertical wrapper for components with rounded border, default padding and slightly orange background.
 * @param param0:React.ComponentProps<"div">
 * @returns 
 */
export default function OrangeWrapper({ className, children, ...props }: React.ComponentProps<"div">) {


    return (
        <div
            className={cn("border-1 border-[#FFE8D1] bg-[#FFF8F0] px-4 py-6 grid gap-4 rounded-xl", className)}
            {...props}
        >
            {children}
        </div>

    );

}