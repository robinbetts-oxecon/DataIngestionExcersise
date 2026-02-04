# Data Ingestion Pairing Exercise

This repo is a small, self-contained **.NET 8** solution intended for a **live pairing** interview.

## Scenario (high level)
You’re working on a data ingestion component that takes batches of provider records and writes them into a repository.

**Desired behaviour**
- Records are identified by a **business key**: `(Provider, Series, AsOfDate)`.
- If duplicates occur **within a batch**, we keep the **last record** for a given business key.
- The ingestion is **idempotent by batch**: re-processing the same `batchId` should not create duplicate writes.
- Writes should be done using a **bulk upsert** pattern (one repository call per ingestion), not per-record calls.

The task is to ensure the desired behaviour is met. The existing code is a starting point.
You can refactor freely, but aim for the behaviour described above.


## Notes
- Everything is in-memory; no external services.
- The repository interface is intentionally tiny.