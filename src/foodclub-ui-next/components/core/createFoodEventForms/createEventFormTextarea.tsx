import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import * as React from "react"

interface CreateEventFormTextareaProps extends React.TextareaHTMLAttributes<HTMLTextAreaElement> {
    labelText: string,
    id: string
}

export default function CreateEventFormTextarea({
    labelText: label,
    id,
    ...props
}: CreateEventFormTextareaProps) {
    return (
        <div className="grid gap-2">
            <Label
                htmlFor={id}
                className="text-xs font-bold text-muted-foreground"
            >
                {label}
            </Label>

            <Textarea
                id={id}
                name={props.name ?? id}
                className="h-48 bg-gray-50 text-sm font-normal text-[#9CA3AF] font-['Poppins']"
                {...props}
            />
        </div>
    )
}
