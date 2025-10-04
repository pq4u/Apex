import Image from "next/image";
import styles from "./page.module.css";
import Link from "next/link";

export default function Home() {
  return (
    <div className="min-h-screen bg-white">
      <main className="flex flex-col items-center justify-center min-h-screen px-4">
        <h1 className="text-6xl font-bold mb-4 text-gray-900">
          Apex
        </h1>
        <p className="text-xl text-gray-600 mb-8">
          F1 statistics, telemetry, and other nerd stuff
        </p>
        <a href="/session-data"
          className="px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white rounded">
          Session Data
        </a>
      </main>
      <footer className="fixed bottom-0 w-full py-4 text-center text-gray-500">
        <a href="https://github.com/pq4u/Apex">GitHub</a>
      </footer>
    </div>
  );
}
