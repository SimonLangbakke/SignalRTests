import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { cn } from "@/lib/utils"
import * as React from "react"

interface CreateEventFormInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  labelText: string,
  id: string,
  wrapperClassName?: string
}

export default function CreateEventFormInput({
  labelText: label,
  id,
  wrapperClassName: wrapperClassName,
  ...props
}: CreateEventFormInputProps) {
  return (
    <div className={cn(wrapperClassName, "flex flex-col gap-2")} >
      <Label
        htmlFor={id}
        className="text-xs font-bold text-muted-foreground"
      >
        {label}
      </Label>

      <Input
        id={id}
        name={props.name ?? id}
        className="h-10 bg-gray-50 text-sm font-normal text-muted-foreground font-['Poppins']"
        {...props}
      />
    </div>
  )
}
