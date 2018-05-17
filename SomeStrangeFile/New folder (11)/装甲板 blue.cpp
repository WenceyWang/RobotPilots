#include <iostream>
#include "opencv2/highgui/highgui.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/core/core.hpp"
using namespace cv;
using namespace std;



int main()
{

	VideoCapture cap("��ԭʼ�ľ�ͷ.avi");
	if (!cap.isOpened())
	{
		cout << "��ȡ����!" << endl;
	}
	Mat imgOriginal;
	double fps;
	char string[10];
	//namedWindow("Camera FPS");
	double t = 0;


	//ÿһ֡��������������֮������ľ���
	vector<double>line_distance;
	vector<Vec3f>judge_distance;


	while (1)
	{
		t = (double)getTickCount();
		if (waitKey(50) == 30)
			break;
		//Ԥ����
		cap >>imgOriginal;
		t = ((double)getTickCount() - t) / getTickFrequency();
		fps = 1.0 / t;


		if (imgOriginal.empty()) break;
		blur(imgOriginal, imgOriginal, Size(3, 3));
		Mat imgHsv2;
		cvtColor(imgOriginal, imgHsv2, COLOR_BGR2HSV);
		Mat mask2;

		//ʶ����ɫ 
		inRange(imgHsv2, Scalar(80 + 6, 40 + 7, 100 - 18), Scalar(120, 255, 255), mask2);
		Mat element2 = getStructuringElement(MORPH_RECT, Size(5, 5));
		morphologyEx(mask2, mask2, MORPH_OPEN, element2);
		morphologyEx(mask2, mask2, MORPH_CLOSE, element2);


		vector<vector<Point>> contours;       //��������������������������ڲ��ǵ�
		findContours(mask2, contours, CV_RETR_EXTERNAL, CV_CHAIN_APPROX_NONE);



		vector<RotatedRect>rectPoint(contours.size());
		vector<int> count;
		int k = 0;
		Mat result = imgOriginal.clone();
		Mat final_result = imgOriginal.clone();


		for (int i = 0; i < contours.size(); i++)
		{
			rectPoint[i] = minAreaRect(contours[i]);
			Point2f fourPoint2f[4];
			rectPoint[i].points(fourPoint2f);


			//���㳤��ע�⣺����ѡȡʵ������Ƕ��йأ�����Ϊ���ϵ����£�
			double height_y = abs(fourPoint2f[1].y - fourPoint2f[0].y);
			double height_x = abs(fourPoint2f[1].x - fourPoint2f[0].x);
			double height = sqrt(pow(height_x, 2) + pow(height_y, 2));
			double width_x = abs(fourPoint2f[1].x - fourPoint2f[2].x);
			double width_y = abs(fourPoint2f[1].y - fourPoint2f[2].y);
			double width = sqrt(pow(width_x, 2) + pow(width_y, 2));

			if (height > width)
			{
				for (int k = 0; k < 4; k++)
				{
					line(imgOriginal, fourPoint2f[k], fourPoint2f[(k + 1) % 4], Scalar(0, 255, 255), 2, 8);
				}
				count.push_back(i);


				vector<Vec4f> line_p(contours.size());  //�������ֱ��
														//vector<vector<Point2f>> ver(contours.size());
				for (int i = 0; i <count.size(); i++)//����������
				{

					Point2f vertices[4];
					rectPoint[i].points(vertices); //��þ�����ĸ�����
					vector<Point2f> vert(2);
					vert[0] = vertices[0];
					vert[1] = vertices[1];

					fitLine(vert, line_p[i], CV_DIST_HUBER, 0, 1e-2, 1e-2);
				}


				vector<Vec3f> cha(count.size()*count.size());
				vector<Vec3f> dis(count.size()*count.size());// ���·�����������ֱ�ߵ����������ĺ�����֮��
				int k = 0;
				int m = 0;
				//int H, S, V;
				for (int i = 0; i < count.size(); i++)
				{
					for (int j = i + 1; j < count.size(); j++)
					{
						double k1 = fabs(line_p[i][1] / line_p[i][0]);
						double k2 = fabs(line_p[j][1] / line_p[j][0]);
						//Point2f vertices_I[4];
						//rectPoint[i].points(vertices_I);
						//Point2f vertices_J[4];
						//rectPoint[j].points(vertices_J);

						//center distance
						double dis_x = rectPoint[i].center.x - rectPoint[j].center.x;
						double dis_y = rectPoint[i].center.y - rectPoint[j].center.y;
						double center_dis = sqrt(pow(dis_x, 2) + pow(dis_y, 2));
						Vec3f dis(i, j, center_dis);
						cha[m] = dis;
						m++;

						if ((fabs(k1 - k2) <1) && fabs(rectPoint[i].center.y - rectPoint[j].center.y) < 5 /* &&(H>=0)&&(H<180)&&(S>= 0) && (S<255) && (V >= 0) && (V<20)*/)
						{
							double dis_x = rectPoint[i].center.x - rectPoint[j].center.x;
							double dis_y = rectPoint[i].center.y - rectPoint[j].center.y;
							double center_dis = sqrt(pow(dis_x, 2) + pow(dis_y, 2));
							Vec3f dis(i, j, center_dis);
							cha[k] = dis;
							k++;
						}
					}
				}

				//�ҳ���Сx
				if (k != 0)
				{
					double min = 999999999999;
					int minI = 0;
					int minJ = 0;
					for (int i = 0; i < k; i++)
					{
						if (cha[i][2] < min)
						{
							min = cha[i][2];
							minI = cha[i][0];
							minJ = cha[i][1];
						}
					}


					Point2f vertices1[4];
					Point2f vertices2[4];
					rectPoint[minI].points(vertices1); //��þ�����ĸ�����
					rectPoint[minJ].points(vertices2); //��þ�����ĸ�����
					for (int k = 0; k < 4; k++)   //������������
					{
						line(result, vertices1[k], vertices1[(k + 1) % 4], Scalar(255, 0, 255));
						line(result, vertices2[k], vertices2[(k + 1) % 4], Scalar(255, 0, 255));
					}

					vector<Point2f>discenter;
					discenter.push_back(rectPoint[minI].center);
					discenter.push_back(rectPoint[minJ].center);
					//line(result, rectPoint[minI].center, rectPoint[minJ].center, Scalar(255, 0, 255));
					double centerdistance = arcLength(discenter, true);
					line_distance.push_back(centerdistance);
					cout << "centerdistace" << centerdistance << endl;
					
					if (centerdistance < 102)
					{

						Point point = (rectPoint[minI].center + rectPoint[minJ].center) * 0.5;
						double x1 = rectPoint[minI].center.x;
						double x2 = rectPoint[minJ].center.x;
						double y1 = rectPoint[minI].center.y;
						double y2 = rectPoint[minJ].center.y;
						line(result, rectPoint[minI].center, rectPoint[minJ].center, Scalar(255, 0, 255));
						double x = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));//��Բ�İ뾶
						circle(result, point, x / 2, Scalar(255, 0, 255));
					}
					
					//���� ���Ҳ�׼ ǰ��֡����Ƚϲ�׼
				/*	if (centerdistance < 102&&line_distance.size()!=0)
					{
						for (int a = 0; a < line_distance.size(); a++)
						{
							for (int b = a + 1; b < line_distance.size(); b++)
							{
								if (fabs(line_distance[b] - line_distance[a]) < 5)
								{
									Point point = (rectPoint[minI].center + rectPoint[minJ].center) * 0.5;
									double x1 = rectPoint[minI].center.x;
									double x2 = rectPoint[minJ].center.x;
									double y1 = rectPoint[minI].center.y;
									double y2 = rectPoint[minJ].center.y;
									line(result, rectPoint[minI].center, rectPoint[minJ].center, Scalar(255, 0, 255));
									double x = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));//��Բ�İ뾶
									circle(result, point, x / 2, Scalar(255, 0, 255));
								}
							}
						}
					}
					
					*/
					//�����������������,����Բ
					/*
					if (centerdistance < 102)
					{
						Point point = (rectPoint[minI].center + rectPoint[minJ].center) * 0.5;
						double x1 = rectPoint[minI].center.x;
						double x2 = rectPoint[minJ].center.x;
						double y1 = rectPoint[minI].center.y;
						double y2 = rectPoint[minJ].center.y;
						line(result, rectPoint[minI].center, rectPoint[minJ].center, Scalar(255, 0, 255));
						double x = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));//��Բ�İ뾶
						circle(result, point, x / 2, Scalar(255, 0, 255));
					}
					*/
					/*Point point = (rectPoint[minI].center + rectPoint[minJ].center) * 0.5;
					double x1 = rectPoint[minI].center.x;
					double x2 = rectPoint[minJ].center.x;
					double y1 = rectPoint[minI].center.y;
					double y2 = rectPoint[minJ].center.y;

					double x = sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));//��Բ�İ뾶
					circle(result, point, x / 2, Scalar(255, 0, 255)); */
				}

			}

		}


		sprintf(string, "%2f", fps);
		std::string fpsString("FPS:");
		fpsString += string;
		putText(result, fpsString, Point(5, 20), FONT_HERSHEY_DUPLEX, 1, Scalar(0, 0, 255));
		
		imshow("Ч����Ƶ", result);
		
		imshow("Original", imgOriginal);
		imshow("color detect", mask2);
		waitKey(30);
	}

/*
	cout << "------------------------------------------" << endl;
	for (int i = 0; i < line_distance.size(); i++)
	{
		cout << "each image's distance= " << line_distance[i] << endl;
	}

	*/
	waitKey(0);
	return 0;
}
