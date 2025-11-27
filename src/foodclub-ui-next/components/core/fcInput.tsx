import { cn } from "@/lib/utils";
import { Input } from "../ui/input";
import { Label } from "../ui/label";


export default function FcInput({ className, type, ...props }: React.ComponentProps<"input">) {
    return (
            <div className="grid gap-2">
                    {/* TODO: continue working on this component */}
                        <Label htmlFor="email" className="text-xs text-muted-gray font-bold">Email</Label>
                        <Input
                            id="email"
                            type={type}
                            placeholder="m@example.com"
                            required
                            className={cn("h-12", className)}

                            {...props}
                        />

                    </div>

    );
}