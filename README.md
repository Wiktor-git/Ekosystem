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

![Screenshot_20251126_124303](https://github.com/user-attachments/assets/fb982400-c975-44a8-9540-73f31fa43054)
![Screenshot_20251126_124328](https://github.com/user-attachments/assets/f290e524-2784-4651-86ba-259f21b9495c)
![Screenshot_20251126_124341](https://github.com/user-attachments/assets/11e8278f-5bf1-4197-a0ac-d5f066fd403d)
![Screenshot_20251126_124454](https://github.com/user-attachments/assets/191f7db3-d2b6-4be9-b181-0d9a48f004e2)
![Screenshot_20251126_124525](https://github.com/user-attachments/assets/2d68e6db-8891-445f-ac92-3dbebdeb3476)
![Screenshot_20251126_124534](https://github.com/user-attachments/assets/f2ef45c0-58df-4d41-8220-3bc6cf7206a8)
![Screenshot_20251126_174154](https://github.com/user-attachments/assets/8b2415cb-eafd-4a35-8b6b-82ea883929df)
![Screenshot_20251126_175350](https://github.com/user-attachments/assets/b5e57a23-7970-4fcd-a5fb-2d01762770cc)
![Screenshot_20251126_175400](https://github.com/user-attachments/assets/f95a550b-42c6-45df-b330-55df63f1e4c6)
![Screenshot_20251126_175427](https://github.com/user-attachments/assets/3afadd88-baa0-40fb-a50a-3f0a815a2c23)


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
