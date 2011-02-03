
import java.util.ArrayList;

public class AtlasPacker
{

	public final static int BEST = 2;
	public final static int FAST = 1;

	public final static int _MAX_WIDTH = 1024;
	public final static int _MAX_HEIGHT = 512;

	public class ResultPack implements Packable
	{
		ResultPack(int width, int height, int x, int y)
		{
			this.x = x;
			this.y = y;
			this.height = height;
			this.width = width;
		}

		ResultPack(ResultPack source)
		{
			this.x = source.x;
			this.y = source.y;
			this.height = source.height;
			this.width = source.width;
		}

		ResultPack(Packable source)
		{
			this.x = source.getX();
			this.y = source.getY();
			this.height = source.getHeight();
			this.width = source.getWidth();
		}

		private int x;
		private int y;
		private int height;
		private int width;

		public int getHeight()
		{
			return height;
		}

		public int getWidth()
		{
			return width;
		}

		public int getX()
		{
			return x;
		}

		public int getY()
		{
			return y;
		}

		public void setX(int x)
		{
			this.x = x;
		}

		public void setY(int y)
		{
			this.y = y;
		}

		public void setWidth(int width)
		{
			this.width = width;
		}

		public void setHeight(int height)
		{
			this.height = height;
		}

	}

	private boolean _ignore_ui_fit;
	private int _num_src;
	private int _source_sum = 0;
	private int _widest_image = 1;

	public void doPacking(ArrayList<Packable> _image_list, int speed)
	{
		if (_image_list.size() == 0)
		{
			return;
		}
		else if (_image_list.size() == 1)
		{
			_image_list.get(0).setX(0);
			_image_list.get(0).setY(0);
			return;
		}
		_num_src = _image_list.size();
		_anchor = new Coord_info[2 * _image_list.size()];
		for (int i = 0; i < _anchor.length; i++)
		{
			_anchor[i] = new Coord_info();
		}

		// get source summary square
		_source_sum = 0;
		for (int a = 0; a < _image_list.size(); a++)
		{
			_source_sum += _image_list.get(a).getHeight() * _image_list.get(a).getWidth();
		}

		ArrayList<ResultPack> arrayList = new ArrayList<ResultPack>(_image_list.size());
		for (int i = 0; i < _image_list.size(); i++)
		{
			ResultPack resultPack = new ResultPack(_image_list.get(i));
			resultPack.setX(0);
			resultPack.setY(0);
			arrayList.add(resultPack);
		}
		ArrayList<ResultPack> arraySortList = new ArrayList<ResultPack>(arrayList);

		int best_width = 10;
		if (speed == BEST)
		{
			best_width = euPacking(arraySortList);
		}
		else if (speed == FAST)
		{
			best_width = fastPacking(arraySortList);
		}

		rectPack(arraySortList, 0, best_width, _ignore_ui_fit, 0);
		
		for (int i = 0; i < _image_list.size(); i++)
		{
			_image_list.get(i).setX(arrayList.get(i).getX());
			_image_list.get(i).setY(arrayList.get(i).getY());
		}
	}

	//////////////////////////////////////////////////////////////////////////
	// PACK IMAGES BY TRYING SEVERAL PREDEFINED SORT FORMULAS
	//////////////////////////////////////////////////////////////////////////

	final static int WIDTH = 0;
	final static int HEIGHT = 1;
	final static int SQUARE = 2;
	final static int SQUAREHEIGHT = 3;
	final static int SQUAREWIDTH = 4;
	final static int SQUAREMWIDTH = 5;
	final static int HEIGHTWIDTH = 6;
	final static int HEIGHTMWIDTH = 7;
	final static int SQUAREMHEIGHT = 8;
	final static int METHODCOUNT = 9;

	int fastPacking(ArrayList<ResultPack> _image_list)
	{
		float best_ratio = 0;
		ResultPack res;
		float ratio;
		final int FAST_MAX_HEIGHT = 1024;
		
		float method_ratio = 0;
		int best_sort = 0;
		int best_width = 0;

		int start = 0;
		int end = METHODCOUNT;

		for (int t = start; t < end; t++)
		{
			method_ratio = 0;
			ArrayList<ResultPack> sort_list = new ArrayList<ResultPack>(_image_list);
			bubbleSort(sort_list, false, t, true);

			int rc = 0;

			int widestImage = _widest_image;
			int startWidth = getApproximate(widestImage);
			for (int r = startWidth; r <= _MAX_WIDTH; r = r << 1)
			{
				boolean ignore_ui_fit = (rc & 0x1) != 0;

				res = rectPack(sort_list, 0, r, ignore_ui_fit, 0);
				
				boolean fits_res_param = true;

				ratio = (res.width != 0 && res.height != 0) ? (float)_source_sum / (float)(res.width * res.height) : 0;

				if ((_MAX_WIDTH != 0 && _MAX_WIDTH < res.width) || (FAST_MAX_HEIGHT != 0 && FAST_MAX_HEIGHT < res.height))
				{
					fits_res_param = false;
				}

				if (ratio > method_ratio && fits_res_param)
				{
					method_ratio = ratio;
				}

				if (ratio > best_ratio && fits_res_param)
				{
					best_ratio = ratio;

					best_width = r;
					_ignore_ui_fit = ignore_ui_fit;
					best_sort = t;
				}
				rc++;
			
				//System.out.println( "size: startWidth " + startWidth);
				//System.out.println( "size: " + res.width  + "x" + res.height + " ratio : " +ratio);
			}
			System.out.printf("packing method: %d, blank space %f\n", t, (1 - method_ratio) * 100);
		}
		System.out.printf("best packing method: %d, best_width: %d,  \n", best_sort, best_width);

		bubbleSort(_image_list, false, best_sort, true);

		return best_width;
	}

	public static int getApproximate(int widestImage) 
	{
		if ((widestImage & (widestImage - 1)) == 0)
			return 1 << (int)((Math.log(widestImage) / Math.log(2)));
		else
			return 1 << (int)((Math.log(widestImage) / Math.log(2)) + 1);
	}

	//////////////////////////////////////////////////////////////////////////
	// SORT IMAGES
	//////////////////////////////////////////////////////////////////////////
	void bubbleSort(ArrayList<ResultPack> image_list, boolean ascending, int md, boolean place_widest_first)
	{
		int sprev = 0;
		int scur = 0;
		int i = 0;

		if (place_widest_first)
		{
			_widest_image = placeWidestFirst(image_list);
			// skip first in sort (widest)
			i = 1;
		}

		for (; i < _num_src; i++)
		{
			for (int j = _num_src - 1; j > i; j--)
			{
				switch (md)
				{
					case WIDTH:
						sprev = image_list.get(j - 1).width;
						scur = image_list.get(j).width;
						break;

					case HEIGHT:
						sprev = image_list.get(j - 1).height;
						scur = image_list.get(j).height;
						break;

					case SQUARE:
						sprev = image_list.get(j - 1).width * image_list.get(j - 1).height;
						scur = image_list.get(j).width * image_list.get(j).height;
						break;

					case SQUAREHEIGHT:
						sprev = image_list.get(j - 1).width * image_list.get(j - 1).height + image_list.get(j - 1).height;
						scur = image_list.get(j).width * image_list.get(j).height + image_list.get(j).height;
						break;

					case SQUAREWIDTH:
						sprev = image_list.get(j - 1).width * image_list.get(j - 1).height + image_list.get(j - 1).width;
						scur = image_list.get(j).width * image_list.get(j).height + image_list.get(j).width;
						break;

					case SQUAREMWIDTH:
						sprev = image_list.get(j - 1).width * image_list.get(j - 1).height + 30 * image_list.get(j - 1).width;
						scur = image_list.get(j).width * image_list.get(j).height + 30 * image_list.get(j).width;
						break;

					case SQUAREMHEIGHT:
						sprev = image_list.get(j - 1).width * image_list.get(j - 1).height + 30 * image_list.get(j - 1).height;
						scur = image_list.get(j).width * image_list.get(j).height + 30 * image_list.get(j).height;
						break;

					case HEIGHTWIDTH:
						sprev = image_list.get(j - 1).width + image_list.get(j - 1).width;
						scur = image_list.get(j).width + image_list.get(j).width;
						break;

					case HEIGHTMWIDTH:
						sprev = image_list.get(j - 1).width + 2 * image_list.get(j - 1).width;
						scur = image_list.get(j).width + 2 * image_list.get(j).width;
						break;
				}

				if ((ascending && (sprev > scur)) || (!ascending && (sprev < scur)))
				{
					exchange(image_list, j - 1, j);
				}
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////
	// PACK IMAGES USING EURISTIC APPROACH
	//////////////////////////////////////////////////////////////////////////
	public int euPacking(ArrayList<ResultPack> _image_list)
	{
		float best_ratio = 0;
		float ratio;
		final int max_seed = 60;
		final int eu_step = 3;
		int best_width = 0;
		ResultPack res;
		final int max_percent = (_MAX_WIDTH != 0) ? _MAX_WIDTH : 200;
		final int per_step = (_MAX_WIDTH != 0) ? 1 : 4;

		int sa = 0;
		int sb = 0;
		int sc = 0;

		for (int a = 0; a < 2 * eu_step; a += eu_step)
		{
			for (int b = 0; b < max_seed; b += eu_step)
			{

				System.out.printf("%d percent complete\n", (b * 100) / max_seed);

				for (int c = 0; c < max_seed; c += eu_step)
				{
					if ((a == 0) && b == 0 && c == 0)
						continue;

					ArrayList<ResultPack> sort_list = new ArrayList<ResultPack>(_image_list);
					euBubbleSort(sort_list, false, a, b, c, true);

					int rc = 0;
					for (int r = (_MAX_WIDTH != 0) ? _widest_image : 0; r <= max_percent; r += per_step)
					{
						boolean fits_res_param = true;
						boolean ignore_ui_fit = ((rc & 0x1) != 0);

						res = (_MAX_WIDTH != 0) ? rectPack(sort_list, 0, r, ignore_ui_fit, 0) : rectPack(sort_list, r, 0, ignore_ui_fit, 0);

						ratio = (res.width != 0 && res.height != 0) ? (float)_source_sum / (float)(res.width * res.height) : 0;

						if (((_MAX_WIDTH != 0) && _MAX_WIDTH < res.width) || ((_MAX_HEIGHT != 0) && _MAX_HEIGHT < res.height))
						{
							fits_res_param = false;
						}

						if (ratio > (best_ratio + 0.000001) && fits_res_param)
						{
							best_ratio = ratio;
							_ignore_ui_fit = ignore_ui_fit;
							sa = a;
							sb = b;
							sc = c;

							best_width = r;
							System.out.printf("best method found: a=%d, b=%d, c=%d, width=%d, blank space %f\n", a, b, c, r, (1 - best_ratio) * 100);
						}
						rc++;
					}
				}
			}
		}

		euBubbleSort(_image_list, false, sa, sb, sc, true);
		return best_width;
	}

	//////////////////////////////////////////////////////////////////////////
	// SORT IMAGES (EURISTIC)
	//////////////////////////////////////////////////////////////////////////
	private int euBubbleSort(ArrayList<ResultPack> image_list, boolean ascending, int a, int b, int c, boolean place_widest_first)
	{
		int sprev;
		int scur;
		int i = 0;
		int _num_src = image_list.size();
		if (place_widest_first)
		{
			_widest_image = placeWidestFirst(image_list);
			// skip first in sort (widest)
			i = 1;
		}

		for (; i < _num_src; i++)
		{
			for (int j = _num_src - 1; j > i; j--)
			{

				sprev = a * (image_list.get(j - 1).getWidth() * image_list.get(j - 1).getHeight()) + b * (image_list.get(j - 1).getWidth()) + c * (image_list
						.get(j - 1).getHeight());
				scur = a * (image_list.get(j).getWidth() * image_list.get(j).getHeight()) + b * (image_list.get(j).getWidth()) + c * (image_list
						.get(j).getHeight());

				if ((ascending && (sprev > scur)) || (!ascending && (sprev < scur)))
				{
					exchange(image_list, j - 1, j);
				}

			}
		}
		return _widest_image;
	}

	//////////////////////////////////////////////////////////////////////////
	// PLACE WIDEST ELEMENT TO THE ZERO-INDEX
	//////////////////////////////////////////////////////////////////////////
	private int placeWidestFirst(ArrayList<ResultPack> image_list)
	{
		int max_width_index = 0;
		int _num_src = image_list.size();

		for (int k = 0; k < _num_src; k++)
		{
			if (image_list.get(k).getWidth() >= _widest_image)
			{
				_widest_image = image_list.get(k).getWidth();
				max_width_index = k;
			}
		}

		int newPlace = 0;
		exchange(image_list, max_width_index, newPlace);
		return _widest_image;

	}

	private void exchange(ArrayList<ResultPack> image_list, int oldPlace, int newPlace)
	{
		ResultPack temp = image_list.get(newPlace);
		image_list.set(newPlace, image_list.get(oldPlace));
		image_list.set(oldPlace, temp);
	}

	//////////////////////////////////////////////////////////////////////////
	// SUB-ALGORYTHM #1 - RECTANGLE X-INTERSECTION
	//////////////////////////////////////////////////////////////////////////
	boolean intersect(int xs1, int xs2, int xd1, int xd2)
	{
		boolean intersect = false;

		if (xs2 >= xd1 && xs2 <= xd2 && xs1 <= xd1)
			intersect = true;
		else if (xd2 >= xs1 && xd2 <= xs2 && xd1 <= xs1)
			intersect = true;
		else if (xd1 >= xs1 && xd1 <= xs2 && xd2 >= xs1 && xd2 <= xs2)
			intersect = true;
		else if (xs1 >= xd1 && xs1 <= xd2 && xs2 >= xd1 && xs2 <= xd2)
			intersect = true;

		return intersect;
	}

	//////////////////////////////////////////////////////////////////////////
	// SUB-ALGORYTHM #2 - RECTANGLE INTERSECTION
	//////////////////////////////////////////////////////////////////////////
	boolean rectIntersect(int x1, int x2, int y1, int y2, int rx1, int rx2, int ry1, int ry2)
	{
		return (rx2 >= x1 && ry2 >= y1 && rx1 <= x2 && ry1 <= y2);
	}

	//////////////////////////////////////////////////////////////////////////
	// SUB-ALGORYTHM #2 - RECTANGLE INTERSECTION
	//////////////////////////////////////////////////////////////////////////
	boolean rectContains(int x1, int x2, int y1, int y2, int rx1, int rx2, int ry1, int ry2)
	{
		return (x1 >= rx1 && y1 >= ry1 && x2 <= rx2 && y2 <= ry2);
	}

	//////////////////////////////////////////////////////////////////////////
	// MAIN PACKING ALGORYTHM
	//////////////////////////////////////////////////////////////////////////
	class Coord_info
	{
		public int x;
		public int y;
		public int yoff;
		public int xoff;
	};

	Coord_info[] _anchor;

	private ResultPack rectPack(ArrayList<ResultPack> image_list, int width_increase_percent, int direct_size, boolean ignore_eu_fit, int eu_fit)
	{
		int row_width;
		int top_height;
		int top_width = 0;
		int fwidth = direct_size;
		int anchor_count = 0;
		boolean anchored;
		boolean fits = false;
		int im1 = 0;
		int im2 = 1;

		// place first two images one after another (vertically)
		image_list.get(im2).setY(image_list.get(im1).getHeight());

		image_list.get(im1).setX(0);
		image_list.get(im1).setY(0);
		image_list.get(im2).setX(0);
		
		row_width = image_list.get(im2).getWidth();
		top_height = image_list.get(im1).getHeight() + image_list.get(im2).getHeight();

		if (image_list.get(im1).getWidth() > fwidth || image_list.get(im2).getWidth() > fwidth)
		{
			// image does not fit limits
			//System.out.println( " image does not fit limits ");
			ResultPack res = new ResultPack(0, 0, anchor_count, 0);
			return (res);
		}
		top_width = image_list.get(im1).getWidth();

		// add 2 first anchor points
		if (fwidth > image_list.get(im1).getWidth())
		{
			_anchor[anchor_count].x = image_list.get(im1).getWidth();
			_anchor[anchor_count].y = 0;
			_anchor[anchor_count].yoff = image_list.get(im1).getHeight();
			_anchor[anchor_count].xoff = fwidth - _anchor[anchor_count].x;

			anchor_count++;
		}
		_anchor[anchor_count].x = image_list.get(im2).getWidth();
		_anchor[anchor_count].y = image_list.get(im1).getHeight();
		_anchor[anchor_count].yoff = image_list.get(im2).getHeight();
		_anchor[anchor_count].xoff = fwidth - _anchor[anchor_count].x;

		anchor_count++;

		if (image_list.get(im2).getWidth() > top_width)
		{
			top_width = image_list.get(im2).getWidth();
		}
		// main algorithm loop
		for (int i = im2 + 1; i < _num_src; i++)
		{
			anchored = false;

			// try to fit image to one of the anchors
			for (int j = 0; j < anchor_count; j++)
			{
				//System.out.println(" try to fit image to one of the anchors ");
				fits = false;

				if (rectContains(_anchor[j].x, _anchor[j].x + image_list.get(i).getWidth(), _anchor[j].y, _anchor[j].y + image_list.get(i)
						.getHeight(), _anchor[j].x, _anchor[j].x + _anchor[j].xoff, _anchor[j].y, _anchor[j].y + _anchor[j].yoff))
				{
					//System.out.println(" inters = true _anchor " + j);
					fits = true;
				}

				boolean euristic_fit = true;

				if (eu_fit != 0)
				{
					euristic_fit = (_anchor[j].yoff > (image_list.get(i).getHeight() / eu_fit));
				}
				else
				{
					// two different euristic calculations. they affect the result greatly.	
					if (!ignore_eu_fit)
					{
						euristic_fit = (_anchor[j].yoff > (image_list.get(i).getHeight() / 10));
					}
					else
					{
						euristic_fit = (_anchor[j].yoff > (image_list.get(i).getHeight() / 5));
					}
				}

				// we have enough room to place image to the anchor
				if ((_anchor[j].x + image_list.get(i).getWidth() <= fwidth) && fits && (euristic_fit)) // image isn't too tall for anchor corner (euristic)
				{
					//System.out.println( " we have enough room to place image to the anchor ");
					image_list.get(i).setY(_anchor[j].y);
					image_list.get(i).setX(_anchor[j].x);

					_anchor[j].x += image_list.get(i).getWidth();

					if (_anchor[j].x >= top_width)
					{
						top_width = _anchor[j].x;
					}

					// add one more anchor
					if (_anchor[j].yoff > image_list.get(i).getHeight())
					{
						//System.out.println( " add one more anchor ");
						anchor_count++;

						// insert it so physically close anchors will be close in the array
						for (int k = anchor_count - 1; k > j + 1; k--)
						{
							_anchor[k].x = _anchor[k - 1].x;
							_anchor[k].y = _anchor[k - 1].y;
							_anchor[k].yoff = _anchor[k - 1].yoff;
							_anchor[k].xoff = _anchor[k - 1].xoff;
						}

						_anchor[j + 1].x = image_list.get(i).getX();
						_anchor[j + 1].y = image_list.get(i).getY() + image_list.get(i).getHeight();
						_anchor[j + 1].yoff = _anchor[j].yoff - image_list.get(i).getHeight();
						_anchor[j + 1].xoff = _anchor[j].xoff;
					}

					_anchor[j].xoff -= image_list.get(i).getWidth();
					_anchor[j].yoff = image_list.get(i).getHeight();

					if (image_list.get(i).getHeight() + image_list.get(i).getY() > top_height)
					{
						top_height = image_list.get(i).getHeight() + image_list.get(i).getY();
					}

					anchored = true;
					break;
				}
			}

			// we wasn't able to fit image to any anchor - place first in a row
			if (!anchored)
			{
				//System.out.println( " we wasn't able to fit image to any anchor - place first in a row ");

				if (image_list.get(i).getWidth() > fwidth)
				{
					//System.out.println( " !!! image doesn't fit limits ");
					// image doesn't fit limits
					ResultPack res = new ResultPack(0, 0, anchor_count, 0);
					return (res);
				}

				image_list.get(i).setY(top_height);
				image_list.get(i).setX(0);
				row_width = image_list.get(i).getWidth();

				if (row_width > top_width)
				{
					top_width = row_width;
				}

				anchor_count++;

				_anchor[anchor_count - 1].x = image_list.get(i).getWidth();
				_anchor[anchor_count - 1].y = image_list.get(i).getY();
				_anchor[anchor_count - 1].yoff = image_list.get(i).getHeight();
				_anchor[anchor_count - 1].xoff = fwidth - _anchor[anchor_count - 1].x;

				if (image_list.get(i).height + image_list.get(i).y > top_height)
				{
					top_height = image_list.get(i).height + image_list.get(i).y;
				}
			}
		}

		//System.out.println( " res anchor_count " + anchor_count);

		ResultPack res = new ResultPack(top_width, top_height, anchor_count, 0);

		return (res);
	}

}
