#include <iostream>
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/core/core.hpp"
using namespace cv;
using namespace std;

int main()
{
	VideoCapture cap("最原始的镜头.avi");
	if (!cap.isOpened())
	{
		cout << "读取错误!" << endl;
	}

	double fps;
	char string[10];
	double t = 0;

	while (1)
	{
		Mat imgOriginal;
		t = (double)getTickCount();
		if (waitKey(50) == 30)
			break;
		//预处理
		cap >> imgOriginal;
		t = ((double)getTickCount() - t) / getTickFrequency();
		fps = 1.0 / t;

		if (imgOriginal.empty())
		{
			break;
		}
			
		/*blur(imgOriginal, imgOriginal, Size(3, 3));
		Mat imgHsv2;
		cvtColor(imgOriginal, imgHsv2, COLOR_BGR2HSV);
		Mat mask2;

		//识别颜色
		inRange(imgHsv2, Scalar(80 + 6, 40 + 7, 100 - 18), Scalar(120, 255, 255), mask2);
		Mat element2 = getStructuringElement(MORPH_RECT, Size(5, 5));
		morphologyEx(mask2, mask2, MORPH_OPEN, element2);
		morphologyEx(mask2, mask2, MORPH_CLOSE, element2);
		*/
		

		sprintf(string, "%2f", fps);
		std::string fpsString("FPS:");
		fpsString += string;
		putText(imgOriginal, fpsString, Point(5, 20), FONT_HERSHEY_DUPLEX, 1, Scalar(0, 0, 255));

		//imshow("效果视频", result);
		imshow("Original", imgOriginal);
		//imshow("color detect", mask2);
		waitKey(10);
	}
	return 0;
}




