import type { Metadata } from "next";
import { Archivo, Geist_Mono } from "next/font/google";
import "./globals.css";

const archivo = Archivo({
  variable: "--font-archivo",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "Apex",
  description: "F1 statistics, telemetry, and other nerd stuff",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className="flex min-h-screen justify-center pt-8 px-2">
        <div className="w-full max-w-4xl">
          {children}
        </div>
      </body>
    </html>
  );
}
