# SelectAid

Windows 10/11 向けの統合型 AAC + PC 操作支援アプリです。視線入力（OS上はマウスとして動作）、ジャイロマウス、外部スイッチを同一の入力抽象で扱い、AAC と PC 操作をシームレスに切り替えられる構成にしています。

## ビルド

```bash
# .NET 8 SDK がインストール済みの Windows で実行
cd SelectAid
 dotnet build SelectAid.sln
```

## 起動

```bash
# 実行ファイルが生成されたら起動
SelectAid\bin\Debug\net8.0-windows\SelectAid.exe
```

## 基本操作

- **Home** から AAC/定型文/文字盤/PC 操作/設定に移動。
- **AAC 画面**で文字入力 → Speak を押すと SAPI で発話。
- **Buzzer** ボタンで即時ブザー。
- **Overlay** で OS へのクリック/スクロール/タブ送出。
- **MouseGrid** で細かい位置選択 → Left/Right Click。

## 支援者設定

- **Supporter** 画面でプロファイル追加/切替。
- **Settings** 画面で自動起動、音声レート/音量を調整。
- **OverlayPanel** で Overlay と MouseGrid の有効/無効。
- **Supporter** 画面で入力モードや電源操作の許可を設定。

## 入力モード

- EyeOnly / Eye+Switch / Eye+Gyro / GyroOnly / SwitchScan をプロファイルに保持。
- 現在の入力モードは画面上部に常時表示。

## テーマ切替

- Friendly / Stylish / Kids の3テーマを標準搭載。
- Settings 画面でテーマを切替（再起動不要）。
- テーマとスケールは独立して適用。

## PC 操作（LINE/YouTube 例）

- LINE や YouTube は外部アプリ/ブラウザを直接開く。
- SelectAid の Overlay から **Left/Right/Double/Scroll/Tab/Back** を送出。
- MouseGrid で細かい UI 操作（動画のシークバー等）を実施。

## バックアップ/復元

- BackupRestore 画面でワンタップバックアップ（zip）を作成。
- 復元は CareLock が無効のときのみ実行可能。

## 復旧手順

- **EmergencyStop**: F12 などのStopキーで即時 Home に戻る。
- **セーフ起動**: 起動時に Shift 長押しで Supporter 画面へ直行。
- **復元**: BackupRestore 画面からバックアップ zip を指定して復元。

## 主要クラス

- `AppStateService`: JSON 永続化と現在プロファイル管理
- `InputRouter`: 視線/スイッチ/キーボード入力を共通アクションに正規化
- `ScanEngine`: 3階層のスキャン状態機械
- `OverlayWindow`: OS に対するクリック/スクロール/キー送出 UI
- `MouseGridWindow`: 画面分割選択による細粒度クリック
- `AacViewModel`: AAC 入力/発話/履歴/予測

## 永続化パス

`%AppData%\SelectAid\` 配下に settings/profiles/keyboardLayouts/phrases/userDict/history/log などを保存します。
