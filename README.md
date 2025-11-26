# Ekosystem Score Calculator 🃏🌿

An **Android app built with .NET MAUI** that helps calculate scores for the card game [**Ekosystem** published by 'nasza księgarnia'](https://nk.com.pl/ekosystem/p3101.html?srsltid=AfmBOopCVQJ4KcQvqMzkcGooi41lRyg2D4SSxdRpzvo7wHLy-cip4QBD) - .  
This project was created as an exercise to combine mobile development with machine learning (YOLO11 Object Detection).

---

## ✨ Features
- Responsive UI that automatically updates when values are changed
- Take photos using your phone’s camera.
- app automatically converts the photo of the cards into a **5x4 grid** based on position of left corners of bounding boxes and displays it using
  - a conversion done with a Yolo11 model running on **ONNX Runtime** with **Android NNAPI** for fast calculations on CPU/GPU.
- Calculate scores once every player is added (calculated with my own script in Ekosystem.cs based on [rules of the game](https://gry.nk.com.pl/wp-content/uploads/2023/04/ekosystem_instrukcja_2023.pdf)).
- Get a **detailed breakdown** of each player’s score.

---

## 🎮 How It Works
1. Add a player.
2. Assign a name.
3. Capture a photo of their card layout.
4. The app converts the photo into a **grid (5 high × 4 wide)** for calculations.
5. Repeat for all players.
6. Tap **"Policz punkty"** to calculate scores.
7. View the breakdown and total score for each player.
8. Celebrate the winner!

---

## 🛠️ Tech Stack
- **.NET MAUI** – Cross-platform app framework (app should also work on IPhone if you change line 26 in DetekcjaKart.cs from NNapi to CoreML).
- **ONNX Runtime** – Executes the YOLO model.
- **YOLO Model** – Detects and classifies card elements for scoring.



## 🧠 Model Training
The YOLO model was trained in two stages:
1. Collected and labeled **15 images** using Label Studio, then trained a **large model**.
2. Used the large model to auto-label **50 additional images**, imported them back into Label Studio.
3. Verified and corrected labels manually.
4. Trained a **smaller, refined model** on the corrected dataset for faster inference on mobile devices.



## 📝 TODO

- Model works very slowly - 30 to 60 seconds on a phone from 2019 - by adding additional ~200 labeled pictures of cards the model could be further shrunk into a **nano** model with **320x320** resolution which would run [up to 16 times faster](https://docs.ultralytics.com/models/yolo11/#overview) than the current **medium** model with **640x640** res (simpler models are more prone to erros). Acomplishing this task would take around 6 hours, but right now I am relatively satisfied with the state of the app as it was mostly done as a programming excercise and i would learn nothing by improving the model.
