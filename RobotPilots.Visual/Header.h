//先把判断斜率屏蔽掉
//分组少一个先手动分好
//Point center=(group[a][0][0], group[a][0][1]);这是个巨坑啊！！千万不能等于啊，要细心啊！！
#include <iostream>
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/core/core.hpp"
using namespace cv;
using namespace std;

bool cmp(const float&a, const float&b)
{
	if (a < b)
		return true;
	else
		return false;
}

//class armour_message
//{
//public:
//	Point point;
//	double x;
//};
//void armour_message::message(Point _point, double _x)
//{
//	point = _point;
//	x = _x;
//}

int main()
{
	Mat imgOriginal = imread("two cars.png");
	blur(imgOriginal, imgOriginal, Size(5, 5));
	Mat imgHsv;
	cvtColor(imgOriginal, imgHsv, COLOR_BGR2HSV);
	Mat mask;

	inRange(imgHsv, Scalar(80 + 6, 40 + 7, 100 - 18), Scalar(120, 255, 255), mask);
	Mat element2 = getStructuringElement(MORPH_RECT, Size(5, 5));
	morphologyEx(mask, mask, MORPH_OPEN, element2);
	morphologyEx(mask, mask, MORPH_CLOSE, element2);

	vector<vector<Point>> contours;
	findContours(mask, contours, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_NONE);
	

	if (contours.size() != 0)
	{
		vector<RotatedRect>rectPoint(contours.size());
		for (int i = 0; i < contours.size(); i++)
		{
			rectPoint[i] = minAreaRect(contours[i]);
			Point2f fourPoint2f[4];
			rectPoint[i].points(fourPoint2f);
			for (int k = 0; k < 4; k++)
			{
				line(imgOriginal, fourPoint2f[k], fourPoint2f[(k + 1) % 4], Scalar(0, 0, 255), 2, 8);
			}
		}
		//斜率
		vector<Vec4f>fit_all_line(contours.size());
		for (int j = 0; j < contours.size(); j++)
		{
			Point2f vertices[4];
			rectPoint[j].points(vertices); //获得矩阵的四个顶点
			vector<Point2f> vert(2);
			vert[0] = vertices[0];
			vert[1] = vertices[1];
			fitLine(vert, fit_all_line[j], CV_DIST_HUBER, 0, 1e-2, 1e-2);
		}

		
		vector<Vec3f> message;
		
		for (int i = 0; i < contours.size(); i++)
		{
			for (int j = i + 1; j < contours.size(); j++)
			{
				double k1 = fabs(fit_all_line[i][1] / fit_all_line[i][0]);
				double k2 = fabs(fit_all_line[j][1] / fit_all_line[j][0]);
				if (/*(fabs(k1 - k2) < 10) &&*/ fabs(rectPoint[i].center.y - rectPoint[j].center.y) < 10)
				{
					RotatedRect minA_rectPoint = rectPoint[i];
					Point2f four_Points_minA[4];
					rectPoint[i].points(four_Points_minA);
					RotatedRect minB_rectPoint = rectPoint[j];
					Point2f four_Points_minB[4];
					rectPoint[j].points(four_Points_minB);
					//minA
					double height_y_minA = abs(four_Points_minA[1].y - four_Points_minA[0].y);
					double height_x_minA = abs(four_Points_minA[1].x - four_Points_minA[0].x);
					double height_minA = sqrt(pow(height_x_minA, 2) + pow(height_y_minA, 2));
					double width_x_minA = abs(four_Points_minA[1].x - four_Points_minA[2].x);
					double width_y_minA = abs(four_Points_minA[1].y - four_Points_minA[2].y);
					double width_minA = sqrt(pow(width_x_minA, 2) + pow(width_y_minA, 2));
					//minB
					double height_y_minB = abs(four_Points_minB[1].y - four_Points_minB[0].y);
					double height_x_minB = abs(four_Points_minB[1].x - four_Points_minB[0].x);
					double height_minB = sqrt(pow(height_x_minB, 2) + pow(height_y_minB, 2));
					double width_x_minB = abs(four_Points_minB[1].x - four_Points_minB[2].x);
					double width_y_minB = abs(four_Points_minB[1].y - four_Points_minB[2].y);
					double width_minB = sqrt(pow(width_x_minB, 2) + pow(width_y_minB, 2));

					//计算minA minB的中心距离
					vector<Point2f>discenter;
					discenter.push_back(rectPoint[i].center);
					discenter.push_back(rectPoint[j].center);
					double centerdistance = arcLength(discenter, true);

					
					
					//判断
					if (true/*(height_minA > width_minA && height_minB > width_minB)||(height_minA == width_minA &&height_minB == width_minB)*/)
					{
						if (centerdistance < 79)
						{
							Point point = (rectPoint[i].center + rectPoint[j].center) * 0.5;
							double x1 = rectPoint[i].center.x;
							double x2 = rectPoint[j].center.x;
							double y1 = rectPoint[i].center.y;
							double y2 = rectPoint[j].center.y;
							double x = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));//求圆的半径
							line(imgOriginal, rectPoint[i].center, rectPoint[j].center, Scalar(0, 0, 255));
							circle(imgOriginal, point, x / 2, Scalar(0, 0, 255));

							//Point point = (rectPoint[i].center + rectPoint[j].center) * 0.5;
							//double x1 = rectPoint[i].center.x;
							//double x2 = rectPoint[j].center.x;
							//double y1 = rectPoint[i].center.y;
							//double y2 = rectPoint[j].center.y;
							//double x = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));//求圆的半径
							//line(imgOriginal, rectPoint[i].center, rectPoint[j].center, Scalar(0, 0, 255));
							//circle(imgOriginal, point, x / 2, Scalar(0, 0, 255));
							Vec3f center0(point.x, point.y, x);
							message.push_back(center0);
							/*Vec3f message(point.x, point.y, x);
							group[i].push_back(message);
							count++;*/
						}
					}
				}
			}
		}
		//排序(X从小到大）
		for (int a = 0; a < message.size(); a++)
		{
			for (int b = a + 1; b < message.size() ; b++)
			{
				if (fabs(message[a][0]>message[a+1][0])/*message[a][2] > message[b][2]*/)
				{
					Vec3f temp = message[a];
					message[a] = message[b];
					message[b] = temp;
				}
			}
		}
		//print
		for (int a = 0; a < message.size(); a++)
		{
			cout << "x: " << message[a][0] << " " << "y: " << message[a][1] <<"distance: "<< message[a][2]<< endl;
		}

		//分组
		vector<vector<Vec3f>> group;
		group.resize(message.size());
		int count = 0;
		for (int i = 0; i < message.size()-1; i++)
		{
			if (fabs(message[i][0] - message[i + 1][0]) < 100 && fabs(message[i][1] - message[i + 1][1] < 50))
			{
				group[count].push_back(message[i + 1]);
				cout << "count:" << count << "x:" << message[i + 1][0] << " " << "y: " << message[i + 1][1] << endl;
			}
			else
			{
				count++;
				group[count].push_back(message[i + 1]);
				cout << "count:" << count << "x:" << message[i + 1][0] << " " << "y: " << message[i + 1][1] << endl;
			}

		}
		cout << count << endl;
		group[0].push_back(message[0]);
		
		//print
		for (int a = 0; a < message.size(); a++)
		{
			for (int b = 0; b<group[a].size(); b++)
			{
				cout<<"count: "<<a<<"x:" << group[a][b][0] << " " << "y: " << group[a][b][1] << endl;
			}
		}
		cout << "------------------" << endl;

		for (int a = 0; a < message.size(); a++)
		{
			if (group[a].size() > 1)
			{
				for (int b = 0 ; b < group[a].size(); b++)
				{
					for (int c = b + 1; c < group[a].size(); c++)
					{
						if (group[a][b][2] > group[a][c][2])
						{
							Vec3f temp = group[a][b];
							 group[a][b]=group[a][c];
							 group[a][c] = temp;
						}
					}
				}
			}
		}

		//print
		cout << "------------------" << endl;
		for (int a = 0; a < message.size(); a++)
		{
			for (int b = 0; b<group[a].size(); b++)
			{
				cout << "count: " << a << "x:" << group[a][b][0] << " " << "y: " << group[a][b][1] << endl;
			}
		}

		//存目标
		float min = 999999;
		float minX = 0;
		float minY = 0;
		vector<Point>target;
		for (int a = 0; a < message.size(); a++)
		{
			for (int b =0;b<group[a].size(); b++)
			{
				
				if (group[a].size() == 1)
				{
					//cout << group[a][0][1] << endl;
					Point center(group[a][0][0], group[a][0][1]);
					cout << center << endl;
					target.push_back(center);
				}

				else if (group[a].size() > 1)
				{
					
					/*if (group[a][b][2] < min)
					{
						min = group[a][b][2];
						minX= group[a][b][0];
						minY = group[a][b][1];
					}*/
					////cout << group[a][0][1]<<endl;
					Point center(group[a][0][0], group[a][0][1]);
					cout << center << endl;
					target.push_back(center);
				}
			}
		}

		for (int i = 0; i < target.size(); i++)
		{
			circle(imgOriginal, target[i], 3, Scalar(0, 255, 255));
		}

	}


	
	////先画出所有灯柱
	//if (contours.size() != 0)
	//{
	//	vector<RotatedRect>rectPoint(contours.size());
	//	for (int i = 0; i < contours.size(); i++)
	//	{
	//		rectPoint[i] = minAreaRect(contours[i]);
	//		Point2f fourPoint2f[4];
	//		rectPoint[i].points(fourPoint2f);
	//		for (int k = 0; k < 4; k++)
	//		{
	//			line(imgOriginal, fourPoint2f[k], fourPoint2f[(k + 1) % 4], Scalar(0, 0, 255), 2, 8);
	//		}
	//	}
	//	
	//	vector<Vec4f> line_p(contours.size());  //获得所有直线
	//											//vector<vector<Point2f>> ver(contours.size());
	//	for (int i = 0; i <contours.size(); i++)//。。。。。
	//	{

	//		Point2f vertices[4];
	//		rectPoint[i].points(vertices); //获得矩阵的四个顶点
	//		vector<Point2f> vert(2);
	//		vert[0] = vertices[0];
	//		vert[1] = vertices[1];

	//		fitLine(vert, line_p[i], CV_DIST_HUBER, 0, 1e-2, 1e-2);
	//	}


	//	for (int i = 0; i < contours.size(); i++)
	//	{
	//		for (int j = i + 1; j < contours.size(); j++)
	//		{
	//			double k1 = fabs(line_p[i][1] / line_p[i][0]);
	//			double k2 = fabs(line_p[j][1] / line_p[j][0]);
	//			double dis_x = rectPoint[i].center.x - rectPoint[j].center.x;
	//			double dis_y = rectPoint[i].center.y - rectPoint[j].center.y;
	//			double center_dis = sqrt(pow(dis_x, 2) + pow(dis_y, 2));
	//			
	//			if ((fabs(k1 - k2) <5) && fabs(rectPoint[i].center.y - rectPoint[j].center.y) <10 )
	//			{
	//				Point point = (rectPoint[i].center + rectPoint[j].center) * 0.5;
	//				double x1 = rectPoint[i].center.x;
	//				double x2 = rectPoint[j].center.x;
	//				double y1 = rectPoint[i].center.y;
	//				double y2 = rectPoint[j].center.y;
	//				double x = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));//求圆的半径
	//				line(imgOriginal, rectPoint[i].center, rectPoint[j].center, Scalar(0, 0, 255));
	//				circle(imgOriginal, point, x / 2, Scalar(0, 0, 255));
	//				
	//			}
	//		}
	//	}
	//}
	imshow("Original", imgOriginal);
	imshow("color detect", mask);
	waitKey(0);
	return 0;
}#pragma once
