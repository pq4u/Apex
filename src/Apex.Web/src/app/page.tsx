import Image from "next/image";
import styles from "./page.module.css";
import Link from "next/link";

export default function Home() {
  return (
    <div className={styles.page}>
      <main className={styles.main}>
        <h1>
          Apex
        </h1>

        <p>
          F1 statistics, telemetry, and other nerd stuff
        </p>

        <div className={styles.ctas}>
          <Link
            className={styles.primary}
            href="/stats"
            rel="noopener noreferrer"
          >
            Stats
          </Link>
          <a
            href="/session-data"
            rel="noopener noreferrer"
            className={styles.secondary}
          >
            Session data
          </a>
        </div>
      </main>
      <footer className={styles.footer}>
        <a>Footer bla bla</a>
      </footer>
    </div>
  );
}
