

# Project structure

This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).


- `app/` — main application folder.
- `components/core/` — project's custom components.
- `components/ui/` — UI components imported from the `shadcn` component library. Example: `pnpm dlx shadcn@latest add button`. See the docs: https://ui.shadcn.com/docs/components

Next.js routing example:
- Route `/createFoodEvent` → `app/createFoodEvent/page.tsx`

# Getting started

## Important — before running the project

Before running the project, add your personal GitHub token to your machine's environment variables as `GITHUB_NODE_AUTH_TOKEN`.

The token is referenced from the `.npmrc` configuration like this:

```
//npm.pkg.github.com/:_authToken=${GITHUB_NODE_AUTH_TOKEN}
```

### How to generate a token

1. Go to https://github.com/settings/tokens/new
2. Choose an expiration period.
3. Select at least the `read:packages` permission. If you plan to publish packages, also select `write:packages`.
4. Generate the token and copy it.
5. Add it to your environment variables. On Windows (PowerShell):

```powershell
[Environment]::SetEnvironmentVariable("GITHUB_NODE_AUTH_TOKEN", "[PasteYourTokenHere]", "User")
```
https://superuser.com/questions/949560/how-do-i-set-system-environment-variables-in-windows-10

On Linux/macOS, add it to your shell profile (for example `~/.bashrc` or `~/.zshrc`).
https://askubuntu.com/questions/58814/how-do-i-add-environment-variables 

For more information about package permissions, see: https://docs.github.com/en/packages/learn-github-packages/about-permissions-for-github-packages

## Run the development server

You can start the development server from VS Code by pressing F5 (see `.vscode/launch.json` for configured launch options), or from the terminal with one of the following commands:

```bash
pnpm dev
# or
npm run dev
# or
yarn dev
# or
bun dev
```

Open http://localhost:3000 in your browser to see the app. The page will auto-refresh as you edit files.

This project uses `next/font` to automatically optimize and load fonts.

## Learn more

- [Next.js Documentation](https://nextjs.org/docs)
- [Learn Next.js](https://nextjs.org/learn)
- [Next.js GitHub repository](https://github.com/vercel/next.js)

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the Vercel platform: https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme

See Next.js deployment docs for details: https://nextjs.org/docs/app/building-your-application/deploying

## Run backend (development)

One-time setup:

1. Install PostgreSQL using your OS package manager or the installer from https://www.postgresql.org/download/.
2. Start the PostgreSQL service.
3. Create the database user and database. Example (run in `psql`):

```sql
CREATE USER fcuser WITH PASSWORD '12345678';
CREATE DATABASE foodclub OWNER fcuser;
```

Repeat-every-run steps (development):

1. From the API project folder, connect to PostgreSQL and make sure the database is available. Example:

```bash
psql -h localhost -U fcuser -d foodclub
```

2. Run the backend in development mode (example for a .NET backend):

```bash
dotnet run --environment Development
```

3. Open the URL shown in the backend console output in your browser.