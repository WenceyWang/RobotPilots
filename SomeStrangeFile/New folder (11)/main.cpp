/*********************************
Copyright: RobotPilots
Author: TangZhiYu
Date: 2017-04-18
Description: This is the demo to use classifier to detect the arrow
*********************************/

#include "head.h"

/** 函数声明 */
void detectAndDisplay(Mat frame);
	
/** 全局变量 */
string arrow_cascade_name = "cascade.xml";
CascadeClassifier arrow_cascade;
string window_name = "Capture - Arrow detection";
RNG rng(12345);

/** @主函数 */
int main(int argc, char **argv)
{
	Mat frame;

	//-- 1. 加载级联分类器文件
	if (!arrow_cascade.load(arrow_cascade_name)){ printf("--(!)Error loading\n"); 
return -1; };


	//-- 2. 打开内置摄像头视频流
	VideoCapture capture(0);
	if (!capture.isOpened())
	{
		return -1;
	}

	bool stop = false;
	while (!stop)
	{
		capture >> frame;

		if (!frame.empty())
		{
			detectAndDisplay(frame);
		}
		else
		{
			printf(" --(!) No captured frame -- Break!"); 
			break;
		}

		int c = waitKey(10);
		if ((char)c == 'c')
		{
			break;
		}
	}
	return 0;
}


/** @函数 detectAndDisplay */
void detectAndDisplay(Mat frame)
{
	vector<Rect> arrows;
	Mat frame_gray;

	// 灰度化和直方图均衡
	cvtColor(frame, frame_gray, CV_BGR2GRAY);
	equalizeHist(frame_gray, frame_gray);

	// 多尺寸检测箭头
	arrow_cascade.detectMultiScale(frame_gray, arrows, 1.1, 3, 0 | CV_HAAR_SCALE_IMAGE, Size(30, 30));

	for (int i = 0; i < arrows.size(); i++)
	{
		Point center(arrows[i].x + arrows[i].width*0.5, arrows[i].y + arrows[i].height*0.5);
		ellipse(frame, center, Size(arrows[i].width*0.5, arrows[i].height*0.5), 0, 0, 360, Scalar(255, 0, 255), 4, 8, 0);
	}

	// 显示结果
	imshow(window_name, frame);
}
