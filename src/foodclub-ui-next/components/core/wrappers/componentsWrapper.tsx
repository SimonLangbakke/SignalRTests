import { cn } from "@/lib/utils";

/**
 * Creates a vertical wrapper for components with rounded border and default padding.
 * @param param0:React.ComponentProps<"div">
 * @returns 
 */
export default function ComponentsWrapper({ className, children, ...props }: React.ComponentProps<"div">) {


    return (
        <div
            className={cn("flex border-1 border-gray-200 p-8 my-8 flex-col gap-4 rounded-xl", className)}
            {...props}
        >
            {children}
        </div>

    );

}