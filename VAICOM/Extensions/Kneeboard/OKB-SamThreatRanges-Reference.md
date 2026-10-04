# OKB SAM Threat Range Reference

This document is a human-readable companion to:

- `VAICOM/Extensions/Kneeboard/OKB-SamThreatRanges.json`

Use this file to validate expected ring radius (`Range Max Assumed`) and label mapping behavior before/after wiring the SA map layer.

## Units and conventions

- Range values are in **NMI**.
- Altitude values are in **feet**.
- Rings should use **Range Max Assumed** as the primary radius for v1.
- `Confidence = low` indicates fallback/assumed values where source data was incomplete (`TBC`).

## Fallback assumptions (when direct data is missing)

| Category | Fallback Max Range (NMI) |
|---|---:|
| MANPADS | 2.5 |
| SHORAD | 7.0 |
| MEDIUM_SAM | 20.0 |
| LONG_SAM | 40.0 |
| NAVAL | 25.0 |

## MANPADS

| System Key | Display Name | Min (NMI) | Max (NMI) | Range Max Assumed (NMI) | Alt Max (ft) | Guidance | Confidence |
|---|---|---:|---:|---:|---:|---|---|
| SA18_GROUSE | SA-18 Grouse | 0.25 | 2.5 | 2.5 | 12000 | IR | high |
| SA24_GRINCH | SA-24 Grinch | 0.25 | 2.5 | 2.5 | 12000 | IR | high |
| STINGER_FIM92 | FIM-92 Stinger | 0.1 | 2.0 | 2.0 | 6500 | IR | high |

## SAM / SHORAD

| System Key | Display Name | Category | Min (NMI) | Max (NMI) | Range Max Assumed (NMI) | Alt Max (ft) | Guidance | Confidence |
|---|---|---|---:|---:|---:|---:|---|---|
| SA2_GUIDELINE | SA-2 Guideline | SAM | - | 28.0 | 28.0 | 82000 | RADAR Command Guidance | medium |
| SA3_GOA | SA-3 Goa | SAM | 3.2 | 13.5 | 13.5 | 65600 | RADAR Command Guidance | high |
| SA5_GAMMON | SA-5 Gammon | SAM | - | - | 130.0 | - | SARH | low |
| SA6_GAINFUL | SA-6 Gainful | SAM | 0.5 | 19.2 | 19.2 | 33000 | Command Guidance, SARH Terminal | high |
| SA8_GECKO | SA-8 Gecko | SHORAD | 0.8 | 7.5 | 7.5 | 21000 | RADAR Command Guidance | high |
| SA9_GASKIN | SA-9 Gaskin | SHORAD | 0.4 | 2.5 | 2.5 | 12000 | IR | high |
| SA10_GRUMBLE | SA-10 Grumble | LONG_SAM | 3.0 | 40.0 | 40.0 | 150000 | SARH | high |
| SA11_GADFLY | SA-11 Gadfly | MEDIUM_SAM | - | 19.2 | 19.2 | 82000 | SARH | medium |
| SA13_GOPHER | SA-13 Gopher | SHORAD | 0.4 | 2.8 | 2.8 | 15000 | IR | high |
| SA15_GAUNTLET | SA-15 Gauntlet | SHORAD | 0.8 | 6.5 | 6.5 | 26000 | EO and RADAR Command Guidance | high |
| SA19_GRISON | SA-19 Grison | SHORAD | 0.0 | 4.0 | 4.0 | 16000 | SACLOS | high |
| AVENGER_ADS | Avenger ADS | SHORAD | 0.1 | 3.7 | 3.7 | 11000 | IR | high |
| M6_LINEBACKER | M6 Linebacker | SHORAD | 0.1 | 2.0 | 2.0 | 11000 | IR | high |
| RAPIER | Rapier | SHORAD | - | 4.6 | 4.6 | 9800 | EO and RADAR guided SACLOS | medium |
| ROLAND_MIM115 | MIM-115 Roland | SHORAD | 0.5 | 3.4 | 3.4 | 19500 | Command Guidance | high |
| CHAPPARAL_MIM72G | MIM-72G Chapparal | SHORAD | 0.1 | 3.0 | 3.0 | 9500 | IR | high |
| HAWK_MIM23 | MIM-23 Hawk | MEDIUM_SAM | 1.0 | 25.6 | 25.6 | 45000 | SARH | high |
| PATRIOT_MIM104 | MIM-104 Patriot | LONG_SAM | 1.6 | 86.0 | 86.0 | 80000 | TVM | high |
| NASAMS | NASAMS | MEDIUM_SAM | - | 8.0 | 8.0 | - | TVM | medium |

## Naval systems

| System Key | Display Name | Ground Equivalent / Basis | Range Max Assumed (NMI) | Confidence |
|---|---|---|---:|---|
| NAVAL_KIROV_SA_N6 | Kirov (SA-N-6 / SA-N-9) | SA-10 class assumption | 48.0 | low |
| NAVAL_SLAVA_SA_N6 | Slava (SA-N-6 / SA-N-4) | SA-10 class assumption | 48.0 | low |
| NAVAL_KUZNETSOV_SA_N9 | Kuznetsov (SA-N-9) | SA-15 class assumption | 6.5 | low |
| NAVAL_KRIVAK_SA_N4 | Krivak (SA-N-4) | SA-8 class assumption | 7.5 | low |
| NAVAL_GRISHA_SA_N4 | Grisha (SA-N-4) | SA-8 class assumption | 7.5 | low |
| NAVAL_TYPE052B_HQ16 | Type 052B (HQ-16) | SA-11 class assumption | 25.0 | low |
| NAVAL_TYPE052C_HQ9 | Type 052C (HQ-9) | SA-10 class assumption | 40.0 | low |
| NAVAL_TYPE054A_HQ16 | Type 054A (HQ-16) | SA-11 class assumption | 25.0 | low |
| NAVAL_TICONDEROGA_SM2 | Ticonderoga (SM-2) | Area-defense naval assumption | 90.0 | low |
| NAVAL_ARLEIGH_BURKE_SM2 | Arleigh Burke (SM-2) | Area-defense naval assumption | 90.0 | low |
| NAVAL_OHP_SM2 | Oliver Hazard Perry (SM-2) | Conservative single-arm assumption | 46.0 | low |
| NAVAL_NIMITZ_RIM7 | Nimitz (RIM-7) | Point-defense assumption | 10.0 | low |

## Validation checklist

- Confirm each AWACS radar/system resolves to the expected `systemKey` via aliases.
- Confirm ring radius equals `rangeNmMaxAssumed` for v1 rendering.
- Confirm unknown systems fall back by category defaults.
- Confirm label text uses `displayName` (system type) and not raw unit name.
- Confirm low-confidence/naval assumptions are visually distinguishable if desired later.
