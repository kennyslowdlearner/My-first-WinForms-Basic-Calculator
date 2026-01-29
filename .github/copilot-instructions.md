# Copilot Instructions

## General Guidelines
- Use mouse-only input (buttons) for interactions rather than keyboard typing.
- Set the calculator display TextBox to ReadOnly to prevent manual text entry.

## User Testing Rules for Calculator UI
- If a number token is a single leading zero, entering a non-zero digit should replace that zero (e.g., 03 -> 3; 0 then (-9) -> -9).
- A decimal point must be allowed only once per number token; prevent inserting multiple decimals even when navigating with left/right.