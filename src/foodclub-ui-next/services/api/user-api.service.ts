import { CreateUserRequestDto } from "@/types/create-user-request.type";
import { UserDetails } from "@/types/user-details.type";

export async function getUserById(userId: string): Promise<UserDetails | null> {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_USERS_API_BASE_URL}/api/User?id=${userId}`,
      {
        method: "GET",
        headers: { "Content-Type": "application/json" },
      }
    );

    if (!res.ok) {
      console.error("Events fetch failed:", res.status, res.statusText);
      return null;
    }

    const data = await res.json();
    return data;
  } catch (err) {
    console.error("Events fetch error:", err);
    return null;
  }
}

export async function createUserOnSignup(
  createUserRequest: CreateUserRequestDto,
  signal?: AbortSignal
): Promise<UserDetails | null> {
  try {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_USERS_API_BASE_URL}/api/User`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(createUserRequest),
        signal,
      }
    );

    if (!res.ok) {
      console.error("User creation failed:", res.status, res.statusText);
      return null;
    }

    const data = await res.json();
    return data as UserDetails;
  } catch (err) {
    if ((err as any)?.name === "AbortError") {
      console.log("createUserOnSignup aborted");
      return null;
    }
    console.error("User creation error:", err);
    return null;
  }
}
